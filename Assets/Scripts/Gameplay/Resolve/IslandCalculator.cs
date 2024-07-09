// Onur Ereren - July 2024
// Popcore case

// Calculates any isolated cells not connected to the top row,
// Thus forming an "island"

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PopsBubble
{
    public class IslandCalculator : IIslandCalculator
    {
        #region REFERENCES
        
        private HexGrid _grid;

        #endregion
        
        #region CONSTRUCTOR
        
        public IslandCalculator()
        {
            _grid = DependencyContainer.Grid;
        }
        
        #endregion
        
        #region METHODS
        
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
            // TODO: Part of the workaround for not being able to send bubble to the top row.
            // There is an extra row above the top row, so now we have to check for the top two rows instead.
            return cell.Coordinates.y >= _grid.GridSize.y - 2;
            //return cell.Coordinates.y >= _grid.GridSize.y - 1;
        }
        
        #endregion
    }

}