// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class HexGrid : MonoBehaviour
    {
        #region REFERENCES

        [SerializeField] private Transform _cellParent;
        [field: SerializeField] public Transform BubbleParent { get; private set; }
        
        private BubblePool _pool;
        private GameFlow _flow;
        
        #endregion
        
        #region VARIABLES
        
        [SerializeField] private Vector2Int _gridSize;
        private float _cellWidth;
        private float _cellHalfWidth;
        private float _rowHeight;

        private int _maxNewCellValue;
        private int _minNewCellValue;
        
        [SerializeField] private GameObject _hexCellPrefab;
        [SerializeField] private GameObject _bubblePrefab;
        
        private Dictionary<Vector2Int, HexCell> _cellMap;

        private bool _firstRowOdd;
        
        #endregion
        
        #region METHODS

        #region Grid Generation

        public void Initialize()
        {
            _pool = DependencyContainer.BubblePool;
            _flow = DependencyContainer.GameFlow;
            
            _cellWidth = GameVar.CellWidth;
            _rowHeight = GameVar.RowHeight();
            _cellHalfWidth = _cellWidth * 0.5f;

            _minNewCellValue = _flow.LevelProfile.MinimumStartingValue;
            _maxNewCellValue = _flow.LevelProfile.MaximumStartingValue;
        }
        
        public void GenerateGrid()
        {
            _cellMap = new Dictionary<Vector2Int, HexCell>();
            
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    Vector2Int coords = new Vector2Int(i, j);
                    Vector2 position = CellPosition(coords);

                    HexCell nextCell = GenerateHexCell(coords, position);
                    
                    _cellMap.Add(coords, nextCell);
                }
            }
        }

        public async UniTask PopulateHexes(int numberOfRows, int minimumPower, int maximumPower)
        {
            _pool.InitializeBubbles(_gridSize.x * _gridSize.y);
            
            int minimumYRow = _gridSize.y - numberOfRows;

            List<UniTask> spawnTasks = new List<UniTask>();
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                if (pair.Key.y >= minimumYRow)
                {
                    int value = Random.Range(minimumPower, maximumPower);
                    spawnTasks.Add(pair.Value.SetStartingData(value));
                }
            }
            await UniTask.WhenAll(spawnTasks);
        }
        
        private HexCell GenerateHexCell(Vector2Int coords, Vector2 pos)
        {
            GameObject hexCell = Instantiate(_hexCellPrefab, _cellParent);
            hexCell.transform.position = CellPosition(coords);
                    
            HexCell cellScript = hexCell.GetComponent<HexCell>();
            cellScript.Initialize(this, coords, CellPosition(coords));

            return cellScript;
        }
        
        #endregion
        
        #region Neighbour Detection
        
        public HexCell[] NeighbourCells(HexCell cell)
        {
            Vector2Int coords = cell.Coordinates;
            return NeighbourCells(coords);
        }

        public HexCell[] NeighbourCells(Vector2Int coords)
        {
            HexCell[] neighbours = new HexCell[6];

            for (int i = 0; i < 6; i++)
            {
                Vector2Int neighbourCoords = NeighbourCoords(coords, i);
                if (_cellMap.TryGetValue(neighbourCoords, out HexCell value))
                    neighbours[i] = value;
            }

            return neighbours;
        }
        
        public CellSearchResult IterateForValue(HexCell startingCell)
        {
            int value = startingCell.Value;
            List<HexCell> valueCells = new List<HexCell>();
            List<HexCell> neighbourCells = new List<HexCell>();
            
            Queue<HexCell> processQueue = new Queue<HexCell>();
            
            processQueue.Enqueue(startingCell);

            while (processQueue.Count > 0)
            {
                HexCell nextCell = processQueue.Dequeue();

                HexCell[] neighbours = NeighbourCells(nextCell);

                for (int i = 0; i < 6; i++)
                {
                    if (neighbours[i] == null || neighbours[i].Value == 0) continue;

                    if (neighbours[i].Value == value && !valueCells.Contains(neighbours[i]))
                    {
                        valueCells.Add(neighbours[i]);
                        processQueue.Enqueue(neighbours[i]);
                        continue;
                    }

                    if (neighbours[i].Value != value && !neighbourCells.Contains(neighbours[i]))
                    {
                        neighbourCells.Add(neighbours[i]);
                    }
                }
            }

            CellSearchResult result = new CellSearchResult();
            result.ValueCells = valueCells;
            result.NeighbourCells = neighbourCells;

            return result;
        }
        
        #endregion
        
        #region Cell Positioning
        
        public Vector2 CellPosition(HexCell cell)
        {
            return CellPosition(cell.Coordinates);
        }
        
        public Vector2 CellPosition(Vector2Int coords)
        {
            float positionX = transform.position.x + (coords.x * _cellWidth) + (OddRow(coords) ? _cellHalfWidth : 0f);
            float positionY = transform.position.y + coords.y * (_rowHeight);

            return new Vector2(positionX, positionY);
        }
        
        // This is a hack. I have to rewrite this later
        public Vector2Int HexCoordinate(Vector2 worldPos)
        {
            Vector2Int finalCoordinates = Vector2Int.zero;
            
            float epsilon = 0.2f;
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    Vector2Int coordinateVector = new Vector2Int(i, j);
                    Vector2 preciseVector = CellPosition(coordinateVector);

                    if ((preciseVector - worldPos).magnitude <= epsilon)
                    {
                        finalCoordinates = coordinateVector;
                    }
                }
            }

            return finalCoordinates;
        }

        public HexCell CellFromCoordinates(Vector2Int coordinates)
        {
            return _cellMap[coordinates];
        }
        
        #endregion
        
        #region Cell Manipulation
        public async UniTask MoveGridDown()
        {
            if (!FirstRowEmpty()) return;

            _firstRowOdd = !_firstRowOdd;
            
            // Transfer all cells down
            for (int i = 1; i < _gridSize.y; i++)
            {
                for (int j = 0; j < _gridSize.x; j++)
                {
                    Vector2Int topCoords = new Vector2Int(j, i);
                    Vector2Int bottomCoords = new Vector2Int(j, i - 1);

                    TransferCellData(topCoords, bottomCoords);
                }
            }

            List<UniTask> dropTasks = new List<UniTask>();
            
            // Spawn new bubbles for the top row
            for (int i = 0; i < _gridSize.x; i++)
            {
                Vector2Int coords = new Vector2Int(i, _gridSize.y - 1);
                dropTasks.Add(_cellMap[coords].SetStartingData(RandomStartingValue()));
            }
            
            // Move the bubbles to new locations
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                dropTasks.Add(pair.Value.UpdateForRowMovement());
            }
            await UniTask.WhenAll(dropTasks);
        }

        public async UniTask MoveGridUp()
        {
            _firstRowOdd = !_firstRowOdd;
            
            // Clear the top row
            for (int i = 0; i < _gridSize.x; i++)
            {
                Vector2Int coords = new Vector2Int(i, _gridSize.y - 1);
                _cellMap[coords].Clear(true);
            }
            
            // Transfer all cells up
            for (int i = _gridSize.y - 2; i >= 0; i--)
            {
                for (int j = 0; j < _gridSize.x; j++)
                {
                    Vector2Int bottomCoords = new Vector2Int(j, i);
                    Vector2Int topCoords = new Vector2Int(j, i + 1);

                    TransferCellData(bottomCoords, topCoords);
                }
            }

            // Move the bubbles to new locations
            List<UniTask> dropTasks = new List<UniTask>();
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                dropTasks.Add(pair.Value.UpdateForRowMovement());
            }
            await UniTask.WhenAll(dropTasks);
        }

        public async UniTask ScrambleGrid()
        {
            List<ScrambleData> scrambleData = new List<ScrambleData>();
            List<HexCell> occupiedCells = new List<HexCell>();

            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                if (pair.Value.Value == 0) continue;

                occupiedCells.Add(pair.Value);
                scrambleData.Add(GenerateScrambleData(pair.Value));
            }

            List<UniTask> scrambleTasks = new List<UniTask>();
            foreach (var cell in occupiedCells)
            {
                int randomIndex = Random.Range(0, scrambleData.Count);
                cell.AssignData(scrambleData[randomIndex]);
                scrambleData.RemoveAt(randomIndex);
                scrambleTasks.Add(cell.UpdateForRowMovement());
            }

            await UniTask.WhenAll(scrambleTasks);

        }
        
        #endregion
        
        #region Utility Methods
        private bool OddRow(Vector2Int coords)
        {
            return coords.y % 2 == (_firstRowOdd ? 1 : 0);
        }
        
        private Vector2Int NeighbourCoords(Vector2Int coords, int index)
        {
            bool oddRow = OddRow(coords);

            switch (index)
            {
                case 0: return coords + new Vector2Int(1, 0);
                case 1: return coords + new Vector2Int(oddRow ? 1 : 0, 1);
                case 2: return coords + new Vector2Int(oddRow ? 0 : -1, 1);
                case 3: return coords + new Vector2Int(-1, 0);
                case 4: return coords + new Vector2Int(oddRow ? 0 : -1, -1);
                case 5: return coords + new Vector2Int(oddRow ? 1 : 0, -1);
                default: 
                    Debug.LogError("neighbour coordinates index out of bounds");
                    return coords;
            }
        }

        private bool FirstRowEmpty()
        {
            for (int i = 0; i < _gridSize.x; i++)
            {
                Vector2Int coords = new Vector2Int(i, 0);
                if (_cellMap[coords].Value != 0) return false;
            }

            return true;
        }

        private int RandomStartingValue()
        {
            return Random.Range(_minNewCellValue, _maxNewCellValue + 1);
        }

        private void TransferCellData(Vector2Int fromCoords, Vector2Int toCoords)
        {
            HexCell targetCell = _cellMap[toCoords];
            HexCell sourceCell = _cellMap[fromCoords];
                    
            targetCell.TransferData(sourceCell);
            sourceCell.Clear();
        }

        private ScrambleData GenerateScrambleData(HexCell cell)
        {
            ScrambleData data = new ScrambleData();
            data.Value = cell.Value;
            data.Bubble = cell.Bubble;
            return data;
        }
        
        #endregion

        #endregion

        #region EDITOR VISUALISATION
        
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {

                Gizmos.color = Color.red;

                foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
                {
                    DrawString(pair.Value.Value.ToString(), CellPosition(pair.Key), Color.green);
                }
            }
        }
        
        // Method to draw strings on Scene camera.
        // Obtained from: https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
        public static void DrawString(string text, Vector3 worldPos, Color? colour = null) {
            UnityEditor.Handles.BeginGUI();
            if (colour.HasValue) GUI.color = colour.Value;
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            UnityEditor.Handles.EndGUI();
        }
        
        #endregion
    }

    public struct CellSearchResult
    {
        public List<HexCell> ValueCells;
        public List<HexCell> NeighbourCells;
    }
    

}