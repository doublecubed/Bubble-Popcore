// Onur Ereren - June 2024
// Popcore case

// Prepares the bubble to be shot, and the bubble after that.

namespace PopsBubble
{
    public class PrepareState : GameState
    {
        #region REFERENCES
        
        private IShootIndicator _shootIndicator;
        private IShootValueCalculator _shootCalculator;
        
        #endregion
        
        #region VARIABLES

        private int _nextShootValue;
        private int _shootValue;
        
        #endregion
        
        #region INITIALIZATION
        
        public PrepareState()
        {
            _shootIndicator = DependencyContainer.ShootIndicator;
            _shootCalculator = DependencyContainer.ShootCalculator;
        }
        
        #endregion
        
        #region METHODS
        
        public override void OnEnter()
        {
            _shootCalculator.CalculateValues();
            int shootValue = _shootCalculator.GetValue();
            int nextValue = _shootCalculator.GetNextValue();
            _shootIndicator.Set(shootValue, nextValue);

            OnStateComplete?.Invoke();
        }
        
        #endregion
    }
}
