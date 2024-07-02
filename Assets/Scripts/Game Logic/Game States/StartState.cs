// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class StartState : GameState
    {
        private HexGrid _grid;
        private int _numberOfRows;
        private int _minimumStartingValue;
        private int _maximumStartingValue;
        
        
        public StartState()
        {
            _grid = DependencyContainer.Grid;
            _numberOfRows = DependencyContainer.GameFlow.LevelProfile.NumberOfRows;
            _minimumStartingValue = DependencyContainer.GameFlow.LevelProfile.MinimumStartingValue;
            _maximumStartingValue = DependencyContainer.GameFlow.LevelProfile.MaximumStartingValue;
        }
        
        public override async void OnEnter()
        {
            _grid.Initialize();
            _grid.GenerateGrid();
            await _grid.PopulateHexes(_numberOfRows, _minimumStartingValue, _maximumStartingValue);

            OnStateComplete?.Invoke();
        }
    }
}