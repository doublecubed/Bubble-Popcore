// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PopsBubble
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        private float _cellWidth;
        private float _cellHalfWidth;
        private float _rowHeight;

        [SerializeField] private GameObject _bubblePrefab;
        
        private Dictionary<Vector2Int, HexCell> _cellMap;
        
        #region METHODS
        
        public void GenerateGrid()
        {
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

        public void PopulateHexes(int numberOfRows, int minimumPower, int maximumPower)
        {
            int minimumYRow = _gridSize.y - numberOfRows;
            
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                if (pair.Key.y >= minimumYRow)
                {
                    int value = Random.Range(minimumPower, maximumPower);
                    pair.Value.Value = value;
                    SpawnBubble(pair.Value);
                    // GameObject bubble = Instantiate(_bubblePrefab, transform);
                    // bubble.transform.position = CellPosition(pair.Key);
                    // bubble.GetComponent<Bubble>().Initialize(pair.Value);
                }
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
            return coords.y % 2 == 1;
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