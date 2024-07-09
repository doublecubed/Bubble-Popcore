// Onur Ereren - July 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class UIController : MonoBehaviour
    {
        #region REFERENCES

        private HexGrid _grid;

        #endregion

        #region MONOBEHAVIOUR

        private void Start()
        {
            _grid = DependencyContainer.Grid;
        }

        #endregion
        
        #region METHODS
        
        #region Button Methods

        public async void ScrambleButton()
        {
            await _grid.ScrambleGrid();
        }

        public async void PushGridUp()
        {
            await _grid.MoveGridUp();
        }
        
        #endregion


        
        #endregion
        
    }
}
