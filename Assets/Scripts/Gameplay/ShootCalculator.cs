// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class ShootCalculator : IShootValueCalculator
    {
        private int _minValue;
        private int _maxValue;

        private int _currentValue;
        private int _nextValue;

        public ShootCalculator(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }


        public void CalculateValues()
        {
            _currentValue = _nextValue == 0 ? RandomShootValue() : _nextValue;
            _nextValue = RandomShootValue();
            
            Debug.Log($"Calculated value si {_currentValue}. Next value is {_nextValue}");
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
            Debug.Log($"Value being calculated between {_minValue} and {_maxValue}");
            
            return Random.Range(_minValue, _maxValue + 1);
        }
    }

}