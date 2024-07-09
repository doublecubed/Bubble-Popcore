// Onur Ereren - June 2024
// Popcore case

// Draws a path with the waypoints given, using the LineRenderer

using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{

    public class LinePathDrawer : MonoBehaviour, IPathDrawer
    {
        #region REFERENCES
        
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private LineRenderer _renderer;

        #endregion
        
        #region VARIABLES
        
        private Vector2 _shootPosition;
        
        #endregion
        
        #region MONOBEHAVIOUR
        
        private void OnEnable()
        {
            _shootPosition = _shootingPoint.position;
        }

        #endregion
        
        #region METHODS
        
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
        
        #endregion
    }
}