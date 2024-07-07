using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace PopsBubble
{
    public class IslandCalculator : IIslandCalculator
    {
        private HexGrid _grid;

        public IslandCalculator()
        {
            _grid = DependencyContainer.Grid;
        }
        
        public List<HexCell> CalculateIslandCells()
        {
            List<HexCell> islandCells = new List<HexCell>();
            
            foreach (KeyValuePair<Vector2Int, HexCell> pair in _grid.CellMap)
            {
                if (IsAnIsland(pair.Value)) islandCells.Add(pair.Value);
            }
            
            return islandCells;
        }

        public bool IsAnIsland(HexCell originalCell)
        {
            // If the cell is already a top cell
            if (IsTopCell(originalCell)) return false;
            
            Queue<HexCell> frontierQueue = new Queue<HexCell>();
            Queue<HexCell> visitedQueue = new Queue<HexCell>();
            
            frontierQueue.Enqueue(originalCell);

            while (frontierQueue.Count > 0)
            {
                HexCell nextCell = frontierQueue.Dequeue();

                if (nextCell.Value == 0) return false;
                if (IsTopCell(nextCell)) return false;
                
                visitedQueue.Enqueue(nextCell);

                HexCell[] neighbourCells = _grid.NeighbourCells(nextCell);
                HexCell[] filteredNeighbours =
                    neighbourCells.Where(cell => cell != null && !frontierQueue.Contains(cell) && !visitedQueue.Contains(cell) && cell.Value != 0).ToArray();

                foreach (HexCell neighbour in filteredNeighbours)
                {
                    frontierQueue.Enqueue(neighbour);
                }
            }
            
            return true;
        }

        private bool IsTopCell(HexCell cell)
        {
            return cell.Coordinates.y >= _grid.GridSize.y - 1;
        }
    }

}