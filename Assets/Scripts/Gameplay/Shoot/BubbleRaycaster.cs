// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PopsBubble
{
    public class BubbleRaycaster : MonoBehaviour
    {
        
        [SerializeField] private Transform _shootingPoint;
        private GameFlow _gameFlow;
        private PlayerInput _input;
        [SerializeField] private HexGrid _grid;
        
        [SerializeField] private GameObject _bubbleGhostPrefab;
        [SerializeField] private GameObject _bubblePrefab;
        
        private HexCell _hitCell;
        private HexCell _fromCell;

        private bool _isShooting;

        public HexCell _targetCell;

        public IShootValueCalculator _shootCalculator;
        
        public Action OnBubbleShot;

        private BubblePool _pool;
        
        private void Start()
        {
            _gameFlow = DependencyContainer.GameFlow;
            _input = GetComponent<PlayerInput>();
            _shootCalculator = DependencyContainer.ShootCalculator;
            _pool = DependencyContainer.BubblePool;
            
            _input.OnMouseButtonUp += ShootSignal;
        }

        public void ResetTargetCell()
        {
            _targetCell = null;
        }
        
        private void ShootSignal()
        {
            if (!(_gameFlow.CurrentGameState() is ShootState)) return; 
            
            OnBubbleShot?.Invoke();
        }

        public async void MakeNewBubble()
        {
            if (_targetCell == null)
            {
                Debug.Log("target cell is null");
                return;
            }
            await _targetCell.SetStartingData(_shootCalculator.GetValue());
        }

        public async UniTask CalculatePop()
        {
            
            while (_targetCell != null)
            {
                ChainSearchResult initialSearchResult = _grid.IterateForValue(_targetCell);
                int chainLength = initialSearchResult.ValueCells.Count;
                int nextValue = _targetCell.Value + (chainLength - 1);

                if (initialSearchResult.ValueCells.Count <= 1) break;
                
                for (int i = 0; i < initialSearchResult.ValueCells.Count; i++)
                {
                    initialSearchResult.ValueCells[i].PopBubble();
                }
                
                HexCell nextChainHead = initialSearchResult.NeighbourCells[0];
                int nextChainLength = 0;
                for (int i = 0; i < initialSearchResult.NeighbourCells.Count; i++)
                {
                    if (initialSearchResult.NeighbourCells[i].Value != nextValue) continue;

                    ChainSearchResult nextSearchResult = _grid.IterateForValue(initialSearchResult.NeighbourCells[i]);
                    if (nextSearchResult.ValueCells.Count > nextChainLength)
                    {
                        nextChainLength = nextSearchResult.ValueCells.Count;
                        nextChainHead = initialSearchResult.NeighbourCells[i];
                    }
                }

                _targetCell = null;
                HexCell[] reverseNeighbours = _grid.NeighbourCells(nextChainHead);

                for (int i = 0; i < 6; i++)
                {
                    if (reverseNeighbours[i] != null && initialSearchResult.ValueCells.Contains(reverseNeighbours[i]))
                    {
                        _targetCell = reverseNeighbours[i];
                        await _targetCell.SetStartingData(nextValue);
                        break;
                    }
                }

            }
            
            ResetTargetCell();
        }

        public async UniTask CalculateIslands()
        {
            
        }
        
        private bool HitsTopBoundary(RaycastHit2D hitInfo)
        {
            return hitInfo.transform.gameObject.layer == 8;
        }
    }
}