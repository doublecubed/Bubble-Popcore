// Onur Ereren - July 2024
// Popcore case

using System.Collections.Generic;

namespace PopsBubble
{
    public interface IIslandCalculator
    {
        public List<HexCell> CalculateIslandCells();
    }
}
