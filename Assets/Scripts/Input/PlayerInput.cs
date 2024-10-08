// Onur Ereren - June 2024
// Popcore case

// Uses the old input system.
// Filters the input based on screen position of mouse(finger)
// So that UI elements can be used without providing input

using System;
using UnityEngine;

namespace PopsBubble
{
    public class PlayerInput : MonoBehaviour
    {
        #region REFERENCES
        
        private GameFlow _gameFlow;
        private Camera _mainCam;
        [SerializeField] private Transform _shooterPosition;

        #endregion
        
        #region VARIABLES
        
        [SerializeField] private Vector2 _inputDetectionBottomLeft;
        [SerializeField] private Vector2 _inputDetectionTopRight;
        
        [field: SerializeField] public Vector2 InputVector { get; private set; }
        
        private bool _fingerDown;
        
        #endregion
        
        #region EVENTS
        
        public Action OnMouseButtonUp;
        
        #endregion
        
        #region MONOBEHAVIOUR
        
        private void Start()
        {
            _gameFlow = FindObjectOfType<GameFlow>();
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (_gameFlow.GameIsRunning)
            {
                SetInputVector();
                CheckForShootSignal();
            }
        }

        #endregion
        
        #region METHODS
        
        private void SetInputVector()
        {
            InputVector = (FingerPos() - (Vector2)_shooterPosition.position).normalized;
        }

        private Vector2 FingerPos()
        {
            Vector2 inputPoint = _mainCam.ScreenToWorldPoint(Input.mousePosition);
            
            if (Input.GetMouseButton(0) && WithinInputArea(inputPoint))
            {
                _fingerDown = true; 
                return _mainCam.ScreenToWorldPoint(Input.mousePosition);
            }
            
            return _shooterPosition.position;

        }

        private void CheckForShootSignal()
        {
            if (Input.GetMouseButtonUp(0) && _fingerDown)
            {
                _fingerDown = false;
                OnMouseButtonUp?.Invoke();
            }
        }

        private bool WithinInputArea(Vector2 input)
        {
            return (input.x < _inputDetectionTopRight.x && input.x > _inputDetectionBottomLeft.x &&
                    input.y < _inputDetectionTopRight.y && input.y > _inputDetectionBottomLeft.y);
        }
        
        #endregion
    }
}