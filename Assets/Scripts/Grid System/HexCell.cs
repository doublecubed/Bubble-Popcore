// Onur Ereren - June 2024
// Popcore case study

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class HexCell
    {
        public Vector2Int Coordinates { get; private set; }
        public int Value { get; set; }

        public Bubble Bubble { get; set; }
        
        public Transform objectTransform;
        
        public HexCell(Vector2Int coords, int value = 0)
        {
            Coordinates = coords;
            Value = value;
        }
    }
}