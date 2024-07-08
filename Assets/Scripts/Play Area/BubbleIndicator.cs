// Onur Ereren - June 2024
// Popcore case

using System;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;

namespace PopsBubble
{
    public class BubbleIndicator : MonoBehaviour, IShootIndicator
    {
        private GameFlow _gameFlow;
        private BubblePool _pool;

        [SerializeField] private Transform _currentPoint;
        [SerializeField] private Transform _nextPoint;
        
        private Bubble _currentBubble;
        private Bubble _nextBubble;

        private CancellationToken _ct;
        
        private void Start()
        {
            _gameFlow = DependencyContainer.GameFlow;
            _pool = DependencyContainer.BubblePool;

            _ct = new CancellationToken();
        }

        public async void Set(int value, int nextValue)
        {
            if (_nextBubble == null)
            {
                _nextBubble = await _pool.Dispense(_nextPoint, _nextPoint.position, value);
            }
            
            _currentBubble = _nextBubble;

            _currentBubble.transform.parent = _currentPoint;

            UniTask[] moveTasks = new UniTask[2];
            moveTasks[0] = _currentBubble.transform.DOScale(Vector2.one, GameVar.BubbleAppearDuration).WithCancellation(_ct);
            moveTasks[1] = _currentBubble.transform.DOMove(_currentPoint.position, GameVar.BubbleAppearDuration).WithCancellation(_ct);
            await UniTask.WhenAll(moveTasks);
            
            _nextBubble = await _pool.Dispense(_nextPoint, _nextPoint.position, nextValue);
        }

        public Bubble CurrentBubble()
        {
            return _currentBubble;
        }

        public Bubble NextBubble()
        {
            return _nextBubble;
        }
    }
}
