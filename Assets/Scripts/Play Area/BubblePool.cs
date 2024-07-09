// Onur Ereren - July 2024
// Popcore case

// Actually spawns the maximum possible number of bubbles on the grid at start.
// A bit wasteful, maybe, but I guess the overhead won't be that much.

using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace PopsBubble
{
    public class BubblePool : MonoBehaviour
    {
        #region REFERENCES
        
        private GameFlow _gameFlow;
        private GameObject _bubblePrefab;
        private HexGrid _grid;

        #endregion
        
        #region VARIABLES
        
        private Queue<Bubble> _bubbleQueue;
        private float _tweenDuration;
        public int TotalBubbleCount { get; private set; }

        private CancellationToken _ct;
        
        #endregion
        
        #region MONOBEHAVIOUR
        
        private void Start()
        {
            _bubblePrefab = DependencyContainer.BubblePrefab;
            
            _gameFlow = DependencyContainer.GameFlow;
            _grid = DependencyContainer.Grid;
            _tweenDuration = GameVar.BubbleAppearDuration;
            _ct = this.GetCancellationTokenOnDestroy();

            TotalBubbleCount = 0;
        }

        #endregion
        
        #region METHODS
        
        public void InitializeBubbles(int numberOfBubbles)
        {
            _bubbleQueue = new Queue<Bubble>();

            for (int i = 0; i < numberOfBubbles; i++)
            {
                SpawnNewBubble();
            }
        }

        public async UniTask<Bubble> Dispense(HexCell cell)
        {
            Bubble bubble = await Dispense(_grid.BubbleParent, _grid.CellPosition(cell), cell.Value);

            return bubble;
        }

        public async UniTask<Bubble> Dispense(Transform parent, Vector2 position, int value)
        {
            if (_bubbleQueue.Count <= 0)
            {
                SpawnNewBubble();
            }
            
            Bubble bubble = _bubbleQueue.Dequeue();
            
            Transform bubbleTransform = bubble.transform;
            bubbleTransform.parent = parent;
            bubbleTransform.position = position;
            
            bubble.PrepareForDispense(value);

            bubbleTransform.localScale = Vector2.zero;
            await bubbleTransform.DOScale(Vector2.one, _tweenDuration).WithCancellation(_ct);

            return bubble;
        }

        public async void Recall(Bubble bubble)
        {
            _bubbleQueue.Enqueue(bubble);

            bubble.transform.parent = transform;
            bubble.transform.position = transform.position;
            bubble.ResetBubble();
        }

        private void SpawnNewBubble()
        {
            TotalBubbleCount++;
            
            GameObject bubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity, transform);
            bubble.name = new string($"Bubble {TotalBubbleCount}");
            Bubble bubbleScript = bubble.GetComponent<Bubble>();
            bubbleScript.Initialize(_gameFlow,this, _grid, (TotalBubbleCount + 1) * 2); // starting from 4 to make way for shadow
            _bubbleQueue.Enqueue(bubbleScript);
        }
        
        #endregion
    }

}