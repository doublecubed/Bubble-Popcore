// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class ChainCalculator : IChainCalculator
    {
        private HexGrid _grid;

        public ChainCalculator()
        {
            _grid = DependencyContainer.Grid;
        }
        
        public ChainSearchResult FindChain(HexCell startingCell)
        {
            int value = startingCell.Value;
            List<HexCell> valueCells = new List<HexCell>();
            List<HexCell> neighbourCells = new List<HexCell>();
            
            Queue<HexCell> processQueue = new Queue<HexCell>();
            
            processQueue.Enqueue(startingCell);

            while (processQueue.Count > 0)
            {
                HexCell nextCell = processQueue.Dequeue();

                HexCell[] neighbours = _grid.NeighbourCells(nextCell);

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

            // if the immediate cell has no neighbours, ValueCells count becomes zero. It should at least include the initial cell
            if (valueCells.Count == 0)
            {
                valueCells.Add(startingCell);
            }
            
            ChainSearchResult result = new ChainSearchResult();
            result.Value = value;
            result.Length = valueCells.Count;
            result.StartingCell = startingCell;
            result.ValueCells = valueCells;
            result.NeighbourCells = neighbourCells;

            return result;
        }

        public List<ChainSearchResult> BurstChain(List<HexCell> startingCells)
        {
            List<ChainSearchResult> results = new List<ChainSearchResult>();

            foreach (HexCell cell in startingCells)
            {
                results.Add(FindChain(cell));
            }

            return results;
        }
        
    }
}
