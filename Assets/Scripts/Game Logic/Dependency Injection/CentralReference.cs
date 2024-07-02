// Onur Ereren
// June 2024

// A very crude Dependency Injection system. This is the MonoBehaviour side, for references

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class CentralReference : MonoBehaviour
    {
        [SerializeField] private GameFlow _gameFlow;
        [SerializeField] private HexGrid _grid;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private BubbleIndicator _bubbleIndicator;
        [SerializeField] private BubbleRaycaster _bubbleRaycaster;
        [SerializeField] private BubblePool _bubblePool;
        [SerializeField] private BubbleMover _bubbleMover;
        
        private void Awake()
        {
            DependencyContainer.Initialize(_grid, _gameFlow, _playerInput, _bubbleIndicator, _bubbleRaycaster, _bubblePool, _bubbleMover);
        }
    }

}