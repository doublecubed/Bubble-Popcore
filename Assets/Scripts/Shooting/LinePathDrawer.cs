// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{

    public class LinePathDrawer : MonoBehaviour, IPathDrawer
    {
        [SerializeField] private Transform _shootingPoint;
        private Vector2 _shootPosition;
        [SerializeField] private LineRenderer _renderer;

        private void OnEnable()
        {
            _shootPosition = _shootingPoint.position;
        }

        public void DrawPath(List<Vector2> points)
        {
            _renderer.positionCount = points.Count + 1;
            
            if (points.Count == 0) return;
            _renderer.SetPosition(0, _shootPosition);

            for (int i = 0; i < points.Count; i++)
            {
                _renderer.SetPosition(i+1, points[i]);                
            }
        }

        public void ClearPath()
        {
            for (int i = 0; i < _renderer.positionCount; i++)
            {
                _renderer.SetPosition(i, _shootPosition);
            }
        }
    }
}