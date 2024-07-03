using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public interface IIslandCalculator
    {
        public List<HexCell> CalculateIslandCells();
    }
}
