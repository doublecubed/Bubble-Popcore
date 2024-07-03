// Onur Ereren - June 2024
// Popcore case

// This state machine is not flexible or modular; it is created for this specific game.
// I normally go a GameObject route for each state so that I can visually see what state is enabled
// on Hierarchy, but since this is a very straightforward progression of states, I did not do that.
// (Also didn't seem very ECS-like :) )

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class GameStateMachine
    {
        #region REFERENCES
        
        private GameState _startState;
        private GameState _prepareState;
        private GameState _shootState;
        private GameState _moveState;
        private GameState _popState;
        private GameState _tidyState;
        private GameState _endState;

        private GameFlow _gameFlow;
        
        #endregion
        
        #region VARIABLES
        
        public GameState CurrentState { get; private set; }
        
        #endregion
        
        #region CONSTRUCTOR

        public GameStateMachine(GameFlow gameFlow)
        {
            _gameFlow = gameFlow;
            
            _startState = new StartState();
            _prepareState = new PrepareState();
            _shootState = new ShootState();
            _moveState = new MoveState();
            _popState = new ResolveState();
            _tidyState = new TidyState();
            _endState = new EndState();
        }
        
        #endregion
        
        #region METHODS
        
        #region Machine Operation
        
        public void StartMachine()
        {
            SubscribeToStateEndEvents();
            
            CurrentState = _startState;
            CurrentState.OnEnter();
        }

        public void StopMachine()
        {
            CurrentState = _endState;
            CurrentState.OnEnter();
        }

        public void SwitchToNextState()
        {
            CurrentState.OnExit();
            CurrentState = NextState(CurrentState);
            CurrentState.OnEnter();
        }

        public void SwitchToState(GameState state)
        {
            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();
        }
        
        #endregion
        
        #region Internal

        private void SubscribeToStateEndEvents()
        {
            _startState.OnStateComplete += SwitchToNextState;
            _prepareState.OnStateComplete += SwitchToNextState;
            _shootState.OnStateComplete += SwitchToNextState;
            _moveState.OnStateComplete += SwitchToNextState;
            _popState.OnStateComplete += SwitchToNextState;
            _tidyState.OnStateComplete += SwitchToNextState;
        }
        
        private GameState NextState(GameState state)
        {
            if (state == _startState) return _prepareState;
            
            if (state == _prepareState) return _shootState;
            if (state == _shootState) return _moveState;
            if (state == _moveState) return _popState;
            if (state == _popState) return _tidyState;
            if (state == _tidyState) return _prepareState;

            if (state == _endState) Debug.LogError("End state is not supposed to be progressed through.");
            return null;
        }

        #endregion
        
        #endregion
    }
}