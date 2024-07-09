// Onur Ereren
// June 2024

// A very crude Dependency Injection system. This is the MonoBehaviour side, for references

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class CentralReference : MonoBehaviour
    {
        [Header("Prefab References")] 
        [SerializeField] private GameObject _bubblePrefab;
        [SerializeField] private GameObject _hexCellPrefab;
        [SerializeField] private GameObject _mergePointPrefab;
        
        [Header("GameObject References")]
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private Transform _ghostBubble;
        [SerializeField] private Transform _moverTrail;
        
        [Header("Script References")]
        [SerializeField] private GameFlow _gameFlow;
        [SerializeField] private HexGrid _grid;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private BubbleIndicator _bubbleIndicator;
        [SerializeField] private BubblePool _bubblePool;
        [SerializeField] private LinePathDrawer _pathDrawer;
        
        private void Awake()
        {
            DependencyContainer.Initialize(_bubblePrefab, _hexCellPrefab, _mergePointPrefab,
                _shootingPoint, _ghostBubble, _moverTrail,
                _grid, _gameFlow, _playerInput, _bubbleIndicator,
                _bubblePool, _pathDrawer);
        }
    }

}