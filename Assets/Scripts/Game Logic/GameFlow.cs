// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class GameFlow : MonoBehaviour
    {
        #region REFERENCES

        public static GameFlow Instance;

        private GameStateMachine _stateMachine;
        
        #endregion
        
        #region VARIABLES

        #region Level

        [field: SerializeField] public LevelProfile LevelProfile { get; private set; }
        
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

            StartStateMachine();
        }

        private void StartStateMachine()
        {
            _stateMachine = new GameStateMachine(this);
            _stateMachine.StartMachine();
        }

        public GameState CurrentGameState()
        {
            return _stateMachine.CurrentState;
        }
        
        #endregion
    }
}