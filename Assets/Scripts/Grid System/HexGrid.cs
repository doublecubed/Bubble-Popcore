// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace PopsBubble
{
    public class HexGrid : MonoBehaviour
    {
        private BubblePool _pool;
        
        [SerializeField] private Vector2Int _gridSize;
        private float _cellWidth;
        private float _cellHalfWidth;
        private float _rowHeight;

        [SerializeField] private GameObject _bubblePrefab;
        
        private Dictionary<Vector2Int, HexCell> _cellMap;

        private bool _firstRowOdd;
        
        #region METHODS

        private void Start()
        {

        }

        public void GenerateGrid()
        {
            _pool = DependencyContainer.BubblePool;
            
            _cellWidth = GameVar.CellWidth;
            _rowHeight = GameVar.RowHeight();
            
            _cellHalfWidth = _cellWidth * 0.5f;
            _cellMap = new Dictionary<Vector2Int, HexCell>();
            
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    Vector2Int coords = new Vector2Int(i, j);
                    HexCell nextCell = new HexCell(coords);
                    _cellMap.Add(coords, nextCell);
                }
            }
        }

        public async UniTask PopulateHexes(int numberOfRows, int minimumPower, int maximumPower)
        {
            _pool.InitializeBubbles(_gridSize.x * _gridSize.y);
            
            int minimumYRow = _gridSize.y - numberOfRows;

            List<UniTask> bubbleTasks = new List<UniTask>();
            
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                if (pair.Key.y >= minimumYRow)
                {
                    int value = Random.Range(minimumPower, maximumPower);
                    pair.Value.Value = value;
                    bubbleTasks.Add(_pool.Dispense(pair.Value));
                    //SpawnBubble(pair.Value);
                }
            }

            await UniTask.WhenAll(bubbleTasks);
        }

        public async UniTask BringBubble(HexCell targetHex)
        {
            GameObject bubble = Instantiate(_bubblePrefab, transform);
            bubble.GetComponent<Bubble>().Initialize(targetHex);
            bubble.transform.position = CellPosition(targetHex.Coordinates);
            bubble.transform.localScale = Vector2.zero;

            await bubble.transform.DOScale(Vector2.one, 0.5f).WithCancellation(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask MoveGridDown()
        {
            for (int i = 0; i < _gridSize.x; i++)
            {
                Vector2Int checkCoordinates = new Vector2Int(i, 0);
                if (_cellMap[checkCoordinates].Value != 0) return;
            }
            
            _firstRowOdd = !_firstRowOdd;
            for (int i = 0; i < _gridSize.y - 1; i++)
            {
                
            }
            
            
        }
        
        public void SpawnBubble(HexCell targetHex)
        {
            GameObject bubble = Instantiate(_bubblePrefab, transform);
            bubble.transform.position = CellPosition(targetHex.Coordinates);
            bubble.GetComponent<Bubble>().Initialize(targetHex);
        }
        
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
        
        public Vector2 CellPosition(Vector2Int coords)
        {
            float positionX = transform.position.x + (coords.x * _cellWidth) + (OddRow(coords) ? _cellHalfWidth : 0f);
            float positionY = transform.position.y + coords.y * (_rowHeight);

            return new Vector2(positionX, positionY);
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

        
        #endregion

        #endregion
        
    }

    public struct CellSearchResult
    {
        public List<HexCell> ValueCells;
        public List<HexCell> NeighbourCells;
    }
}