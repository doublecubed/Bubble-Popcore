using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{

    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private int _startingRows;  //counted from the top
        [SerializeField] private int _maximumStartingPower; //What power of 2 can a cell take at the start
        
        [SerializeField] private HexGrid _grid;

        public bool GameIsRunning { get; private set; }
        
        private void Start()
        {
            StartGame();
        }


        public void StartGame()
        {
            GameIsRunning = true;
            
            _grid.GenerateGrid();
            _grid.PopulateHexes(_startingRows, _maximumStartingPower);
        }


    }
}