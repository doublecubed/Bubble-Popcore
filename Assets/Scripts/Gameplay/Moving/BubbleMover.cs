// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace PopsBubble
{
    public class BubbleMover : MonoBehaviour, IPathMover
    {
        [SerializeField] private Transform _movingItem;
        [SerializeField] private Transform _startingPoint;
        
        public async UniTask MoveOnPath(Transform target, List<Vector2> waypoints)
        {
            _movingItem.position = _startingPoint.position;
            
            float waypointDuration = GameVar.BubbleMoveDuration / waypoints.Count;
            
            for (int i = 0; i < waypoints.Count; i++)
            {
                await _movingItem.DOMove(waypoints[i], waypointDuration)
                    .WithCancellation(this.GetCancellationTokenOnDestroy());
            }    
        }
    }
}
