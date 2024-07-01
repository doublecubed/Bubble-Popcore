// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class PrepareState : GameState
    {
        private IShootIndicator _shootIndicator;
        private IShootValueCalculator _shootCalculator;
        
        private int _maximumShootValue;

        private int _nextShootValue;
        private int _shootValue;
        
        public PrepareState()
        {
            _shootIndicator = DependencyContainer.ShootIndicator;
            _shootCalculator = DependencyContainer.ShootCalculator;
            _maximumShootValue = DependencyContainer.GameFlow.LevelProfile.MaximumShootValue;
        }
        
        public override void OnEnter()
        {
            Debug.Log("Entering prepare state");
            
            _shootCalculator.CalculateValues();
            int shootValue = _shootCalculator.GetValue();
            int nextValue = _shootCalculator.GetNextValue();
            _shootIndicator.Set(shootValue, nextValue);

            OnStateComplete?.Invoke();
        }
    }
}
