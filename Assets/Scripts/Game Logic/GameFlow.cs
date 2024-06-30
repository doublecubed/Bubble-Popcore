

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{

    public class GameFlow : MonoBehaviour
    {
        #region REFERENCES

        public static GameFlow Instance;
        
        [SerializeField] private HexGrid _grid;

        private GameStateMachine _stateMachine;
        
        #endregion
        
        #region VARIABLES

        #region Level

        [SerializeField] private LevelProfile _levelProfile;//What power of 2 can a cell take at the start
        
        #endregion
        
        public bool GameIsRunning { get; private set; }
        
        #endregion


        #region MONOBEHAVIOUR

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            _stateMachine.CurrentState.OnUpdate();
        }

        #endregion
        
        #region METHODS
        
        public void StartGame()
        {
            GameIsRunning = true;

            GenerateStateMachine();
            GeneratePlayArea();
        }

        private void GenerateStateMachine()
        {
            _stateMachine = new GameStateMachine();
            _stateMachine.StartMachine();
        }

        private void GeneratePlayArea()
        {
            _grid.GenerateGrid();
            _grid.PopulateHexes(_levelProfile.NumberOfRows, _levelProfile.MinimumStartingValue);
        }
        
        public GameState CurrentGameState()
        {
            return _stateMachine.CurrentState;
        }
        
        #endregion
    }
}