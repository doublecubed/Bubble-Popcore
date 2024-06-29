// Onur Ereren - June 2024
// Popcore Case Study

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
        [SerializeField] private float _cellWidth;
        private float _cellHalfWidth;

        [SerializeField] private GameObject _bubblePrefab;
        
        private Dictionary<Vector2Int, HexCell> _cellMap;
        
        public void GenerateGrid()
        {
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

        public void PopulateHexes(int numberOfRows, int maximumPower)
        {
            int minimumYRow = _gridSize.y - numberOfRows;
            
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _cellMap)
            {
                if (pair.Key.y >= minimumYRow)
                {
                    int value = Random.Range(1, maximumPower);
                    pair.Value.Value = value;
                    GameObject bubble = Instantiate(_bubblePrefab, transform);
                    bubble.transform.position = CellPosition(pair.Key);
                    bubble.GetComponent<Bubble>().Initialize(pair.Value);
                }
            }
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
            float positionY = transform.position.y + coords.y * (_cellHalfWidth * Mathf.Sqrt(3));

            return new Vector2(positionX, positionY);
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

    }
}