// Onur Ereren - June 2024
// Popcore case

// Entrance point of the game. Starts and runs the state machine.
// Contains the Level information and Visual style information
// Also responsible of pausing the game (when implemented).

using UnityEngine;

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
        [field: SerializeField] public VisualProfile VisualProfile { get; private set; }
        
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

        public Color ColorByValue(int value)
        {
            int modulus = (value - 1) % VisualProfile.BubbleColors.Length;
            return VisualProfile.BubbleColors[modulus];
        }
        
        #endregion
    }
}