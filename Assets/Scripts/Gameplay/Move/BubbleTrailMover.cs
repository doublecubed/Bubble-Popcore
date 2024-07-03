// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class BubbleTrailMover : IPathMover
    {
        private Transform _shootingPoint;
        private HexGrid _grid;
        private CancellationToken _ct;

        private float _trailMoveSpeed;
        
        public BubbleTrailMover()
        {
            _grid = DependencyContainer.Grid;
            _shootingPoint = DependencyContainer.ShootingPoint;
            _ct = new CancellationToken();

            _trailMoveSpeed = GameVar.BubbleTrailMoveSpeed;
        }
        
        public async UniTask MoveOnPath(Transform target, List<Vector2> waypoints)
        {
            target.position = _shootingPoint.position;
           
            float[] waypointDuration = WaypointDuration(waypoints);
            
            for (int i = 0; i < waypoints.Count; i++)
            {
                await target.DOMove(waypoints[i], waypointDuration[i]).SetEase(Ease.Linear)
                    .WithCancellation(_ct);
            }    
            
            target.position = _shootingPoint.position;
        }

        private float[] WaypointDuration(List<Vector2> waypoints)
        {
            float[] durations = new float[waypoints.Count];

            List<Vector2> points = new List<Vector2>();
            points.Add(_shootingPoint.position);
            points.AddRange(waypoints);
            
            for (int i = 0; i < points.Count - 1; i++)
            {
                float segmentLength = SegmentLength(points[i + 1], points[i]);
                durations[i] = segmentLength / _trailMoveSpeed;
            }

            return durations;
        }

        private float SegmentLength(Vector2 pointOne, Vector2 pointTwo)
        {
            return (pointOne - pointTwo).magnitude;
        }
    }
}
