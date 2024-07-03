// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class ShootValueCalculator : IShootValueCalculator
    {
        private int _minValue;
        private int _maxValue;

        private int _currentValue;
        private int _nextValue;

        public ShootValueCalculator(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }


        public void CalculateValues()
        {
            _currentValue = _nextValue == 0 ? RandomShootValue() : _nextValue;
            _nextValue = RandomShootValue();
        }

        public int GetValue()
        {
            return _currentValue;
        }

        public int GetNextValue()
        {
            return _nextValue;
        }

        private int RandomShootValue()
        {
            return Random.Range(_minValue, _maxValue + 1);
        }
    }

}