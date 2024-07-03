// Onur Ereren - July 2024
// Popcore case

// Actually spawns the maximum possible number of bubbles on the grid.
// A bit wasteful, maybe, but I guess the overhead won't be that much.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace PopsBubble
{
    public class BubblePool : MonoBehaviour
    {
        [SerializeField] private GameObject _bubblePrefab;
        private Queue<Bubble> _bubbleQueue;
        private HexGrid _grid;
        private float _tweenDuration;
        private CancellationToken _ct;
        
        private void Start()
        {
            _grid = DependencyContainer.Grid;
            _tweenDuration = GameVar.BubbleAppearDuration;
            _ct = this.GetCancellationTokenOnDestroy();
        }

        public void InitializeBubbles(int numberOfBubbles)
        {
            _bubbleQueue = new Queue<Bubble>();

            for (int i = 0; i < numberOfBubbles; i++)
            {
                GameObject bubble = Instantiate(_bubblePrefab, transform.position, Quaternion.identity, transform);
                Bubble bubbleScript = bubble.GetComponent<Bubble>();
                bubbleScript.SetReferences(this, _grid);
                _bubbleQueue.Enqueue(bubbleScript);
            }
        }

        public async UniTask Dispense(HexCell cell)
        {
            Bubble nextBubble = _bubbleQueue.Dequeue();
            cell.Bubble = nextBubble;
            
            Transform bubbleTransform = nextBubble.transform;
            bubbleTransform.parent = _grid.BubbleParent;
            bubbleTransform.position = _grid.CellPosition(cell);
            
            nextBubble.Initialize(new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f), cell.Value);

            bubbleTransform.localScale = Vector2.zero;
            await bubbleTransform.DOScale(Vector2.one, _tweenDuration).WithCancellation(_ct);
        }

        public async void Recall(Bubble bubble, bool animate = false)
        {
            _bubbleQueue.Enqueue(bubble);

            bubble.transform.parent = transform;
            bubble.transform.position = transform.position;
        }
        
        
    }

}