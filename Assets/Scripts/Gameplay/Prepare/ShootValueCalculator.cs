// Onur Ereren - July 2024
// Popcore case

// Calculates the current and next shoot values

using UnityEngine;

namespace PopsBubble
{
    public class ShootValueCalculator : IShootValueCalculator
    {
        #region VARIABLES
        
        private int _minValue;
        private int _maxValue;

        private int _currentValue;
        private int _nextValue;

        #endregion
        
        #region CONSTRUCTOR
        
        public ShootValueCalculator(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        #endregion

        #region METHODS
        
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
        
        #endregion
    }

}