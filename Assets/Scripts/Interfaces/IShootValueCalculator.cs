// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public interface IShootValueCalculator
    {
        public void CalculateValues();

        public int GetValue();

        public int GetNextValue();
    }
}
