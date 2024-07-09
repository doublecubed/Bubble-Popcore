// Onur Ereren - June 2024
// Popcore case

// This is where the GameStateMachine starts from. Creates and populates the grid.

namespace PopsBubble
{
    public class StartState : GameState
    {
        #region REFERENCES
        
        private HexGrid _grid;
        
        #endregion
        
        #region VARIABLES
        
        private int _numberOfRows;
        private int _minimumStartingValue;
        private int _maximumStartingValue;
        
        #endregion
        
        #region CONSTRUCTOR
        
        public StartState()
        {
            _grid = DependencyContainer.Grid;
            _numberOfRows = DependencyContainer.GameFlow.LevelProfile.NumberOfRows;
            _minimumStartingValue = DependencyContainer.GameFlow.LevelProfile.MinimumStartingValue;
            _maximumStartingValue = DependencyContainer.GameFlow.LevelProfile.MaximumStartingValue;
        }
        
        #endregion
        
        #region METHODS
        
        public override async void OnEnter()
        {
            _grid.Initialize();
            _grid.GenerateGrid();
            await _grid.PopulateHexes(_numberOfRows, _minimumStartingValue, _maximumStartingValue);

            OnStateComplete?.Invoke();
        }
        
        #endregion
    }
}