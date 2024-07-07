// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public interface IChainCalculator
    {
        public ChainSearchResult FindChain(HexCell startingCell);

        public List<ChainSearchResult> BurstChain(List<HexCell> startingCells);
    }

    public struct ChainSearchResult
    {
        public int Value;
        public int Length;
        public HexCell StartingCell;
        public List<HexCell> ValueCells;
        public List<HexCell> NeighbourCells;
    }
}