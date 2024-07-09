// Onur Ereren - June 2024
// Popcore case

// Tidies up the grid after resolution.
// Currently only tasked with dropping down the grid, but should also check for loss condition or level ups

using UnityEngine;
using System.Threading;

namespace PopsBubble
{
    public class TidyState : GameState
    {
        #region REFERENCES
        
        private GameFlow _gameFlow;
        private HexGrid _grid;
        
        #endregion
        
        #region VARIABLES
        
        private int _dropdownFrequency;
        
        #endregion
        
        #region CONSTRUCTOR
        
        public TidyState()
        {
            _gameFlow = DependencyContainer.GameFlow;
            _grid = DependencyContainer.Grid;

            _dropdownFrequency = _gameFlow.LevelProfile.DropdownFrequency;
        }
        
        #endregion

        #region METHODS
        
        public override async void OnEnter()
        {
            int moveDown = Random.Range(0, _dropdownFrequency);
            
            if (moveDown == 0) await _grid.MoveGridDown();

            OnStateComplete?.Invoke();
        }
        
        #endregion
    }
}
