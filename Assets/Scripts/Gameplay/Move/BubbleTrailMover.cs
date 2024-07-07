// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor.Timeline.Actions;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class BubbleTrailMover : IPathMover
    {
        private Transform _shootingPoint;
        private HexGrid _grid;
        private CancellationToken _ct;

        private float _trailMoveSpeed;

        private Transform _movingTransform;
        private TrailRenderer _trailRenderer;
        
        public BubbleTrailMover()
        {
            _grid = DependencyContainer.Grid;
            _shootingPoint = DependencyContainer.ShootingPoint;
            _movingTransform = DependencyContainer.MoverTrail;
            _trailRenderer = _movingTransform.GetComponentInChildren<TrailRenderer>();
            _ct = new CancellationToken();

            _trailMoveSpeed = GameVar.BubbleTrailMoveSpeed;
        }
        
        public async UniTask MoveOnPath(List<Vector2> waypoints)
        {
            _movingTransform.position = _shootingPoint.position;
           
            float[] waypointDuration = WaypointDuration(waypoints);
            
            for (int i = 0; i < waypoints.Count; i++)
            {
                await _movingTransform.DOMove(waypoints[i], waypointDuration[i]).SetEase(Ease.Linear)
                    .WithCancellation(_ct);
            }

            _trailRenderer.enabled = false;
            _movingTransform.position = _shootingPoint.position;
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

        public void ResetPosition()
        {

            _trailRenderer.enabled = true;
        }
    }
}
