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
    }

    public struct ChainSearchResult
    {
        public List<HexCell> ValueCells;
        public List<HexCell> NeighbourCells;
    }
}