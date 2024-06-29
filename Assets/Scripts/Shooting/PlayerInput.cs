using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class PlayerInput : MonoBehaviour
    {
        private GameFlow _gameFlow;
        private Camera _mainCam;
        [SerializeField] private Transform _shooterPosition;
        
        [field: SerializeField] public Vector2 InputVector { get; private set; }

        private void Start()
        {
            _gameFlow = FindObjectOfType<GameFlow>();
            _mainCam = Camera.main;
        }

        private void Update()
        {
            if (_gameFlow.GameIsRunning)
                SetInputVector();
        }

        private void SetInputVector()
        {
            InputVector = (FingerPos() - (Vector2)_shooterPosition.position).normalized;
        }

        private Vector2 FingerPos()
        {
            return Input.GetMouseButton(0) ? _mainCam.ScreenToWorldPoint(Input.mousePosition) : _shooterPosition.position;
        }
    }
}