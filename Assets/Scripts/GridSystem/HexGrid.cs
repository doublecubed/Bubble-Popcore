// Onur Ereren - June 2024
// Popcore Case Study

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        private const float CellWidth = 1f;
        private float _cellHalfWidth;
        
        [SerializeField] private HexCell[][] _cells;

        [SerializeField] private Dictionary<Vector2Int, HexCell> _cellMap;

        [SerializeField] private GameObject _cellPrefab;
        
        private void OnEnable()
        {
            _cellHalfWidth = CellWidth * 0.5f;

            _cellMap = new Dictionary<Vector2Int, HexCell>();
            
            for (int i = 0; i < _gridSize.x; i++)
            {
                for (int j = 0; j < _gridSize.y; j++)
                {
                    Vector2Int coordinates = new Vector2Int(i, j);
                    HexCell hexCell = new HexCell(coordinates);
                    _cellMap.Add(coordinates, hexCell);

                    Instantiate(_cellPrefab, CellPosition(coordinates), Quaternion.identity);
                }
            }
        }


        public HexCell HexCell(Vector2 position)
        {
            int initialX = Mathf.Clamp(Mathf.FloorToInt(position.x), 0, _gridSize.x);
            int initialY = Mathf.Clamp(Mathf.FloorToInt(position.y), 0, _gridSize.y);
            Vector2Int initialCoords = new Vector2Int(initialX, initialY);
            HexCell initialCell = _cellMap[initialCoords];
            
            List<HexCell> cellChecklist = Neighbours(initialCell);
            cellChecklist.Add(initialCell);

            HexCell finalCell = initialCell;
            float distance = Mathf.Infinity;

            for (int i = 0; i < cellChecklist.Count; i++)
            {
                float tempDistance = Vector2.Distance(position, CellPosition(cellChecklist[i].Coordinates));
                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    finalCell = cellChecklist[i];
                }
            }

            return finalCell;
        }
        
        public HexCell HexCell(Vector2Int coords)
        {
            return _cellMap[coords];
        }

        public List<HexCell> Neighbours(HexCell cell)
        {
            List<HexCell> neighbourCells = new List<HexCell>();

            for (int i = 0; i < 6; i++)
            {
                Vector2Int neighbourCoords = NeighbourCoords(cell.Coordinates, i);

                if (_cellMap.TryGetValue(neighbourCoords, out HexCell neighbourCell))
                    neighbourCells.Add(neighbourCell);
            }

            return neighbourCells;
        }
        
        public Vector2 CellPosition(Vector2Int coords)
        {
            float horizontalPos = (coords.x * CellWidth) + (OddRow(coords) ? _cellHalfWidth : 0f);
            float verticalPos = coords.y * RowDistance();

            return new Vector2(horizontalPos, verticalPos);
        }

        private float RowDistance()
        {
            return (3f * _cellHalfWidth) / (Mathf.Sqrt(3f));
        }

        private bool OddRow(Vector2Int coordinates)
        {
            return coordinates.y % 2 == 1;
        }

        private Vector2Int NeighbourCoords(Vector2Int coords, int index)
        {
            bool oddRow = OddRow(coords);
            
            switch (index)
            {
                case 0: 
                    return coords + new Vector2Int(1, 0);
                case 1:
                    return coords + new Vector2Int(oddRow ? 1 : 0, 1);
                case 2:
                    return coords + new Vector2Int(oddRow ? 0 : -1, 1);
                case 3:
                    return coords + new Vector2Int(-1, 0);
                case 4:
                    return coords + new Vector2Int(oddRow ? 0 : -1, -1);
                case 5:
                    return coords + new Vector2Int(oddRow ? 1 : 0, -1);
                default:
                    Debug.LogError("Neighbour coordinate index is out of bounds");
                    break;
            }

            return coords;
        }

        private Vector2 RoundToGrid(Vector2 value)
        {
            float horizontal = Mathf.Clamp(((Mathf.Round(value.x) / CellWidth) * CellWidth), 0f, CellWidth * _gridSize.x);
            float vertical = Mathf.Clamp(((Mathf.Round(value.y) / RowDistance()) * RowDistance()), 0f,
                RowDistance() * _gridSize.y);

            return new Vector2(horizontal, vertical);
        }
    }
}