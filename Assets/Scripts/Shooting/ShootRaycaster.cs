// Onur Ereren - July 2024
// Popcore case

// Calculates the path of the shoot and the end HexCell.
// Also makes the path be drawn on the screen.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopsBubble
{
    public class ShootRaycaster : IRaycaster
    {
        #region REFERENCES
        
        private readonly Transform _shootingPoint = DependencyContainer.ShootingPoint;
        private readonly HexGrid _grid = DependencyContainer.Grid;

        #endregion
        
        #region VARIABLES
        
        #region Constant
        
        private const int _leftWallLayer = 6;
        private const int _rightWallLayer = 7;
        private const int _topWallLayer = 8;
        private const int _cellLayer = 9;
        
        #endregion
        
        #endregion

        public ShootRaycastResult ShootRaycast(Vector2 direction)
        {
            ShootRaycastResult result = new ShootRaycastResult();
            
            // There is no input
            if (direction == Vector2.zero) return result;
            
            // The angle is too low. Causes a lot of zig-zagging. Too much drama.
            float directionAngle = Vector2.Angle(direction, Vector2.right);
            if (directionAngle < 10 || directionAngle > 170) return result; 
            
            Vector2 firstDirection = direction;

            RaycastHit2D firstHitInfo = Physics2D.Raycast(_shootingPoint.position, firstDirection);

            result.HitPoints = new List<Vector2> { firstHitInfo.point };

            int hitLayer = Layer(firstHitInfo);
            
            // Here will be the code of what happens if it hits top boundary directly

            while (hitLayer == _leftWallLayer || hitLayer == _rightWallLayer)
            {
                Vector2 bounceDirection = BounceDirection(firstDirection);
                LayerMask bounceMask = BounceLayerMask(hitLayer);
                
                RaycastHit2D bounceHitInfo =
                    Physics2D.Raycast(firstHitInfo.point, bounceDirection, Mathf.Infinity, bounceMask);
                
                result.HitPoints.Add(bounceHitInfo.point);

                firstDirection = bounceDirection;
                firstHitInfo = bounceHitInfo;
                hitLayer = Layer(bounceHitInfo);
            }
            
            HexCell hitCell = _grid.CellFromCoordinates(_grid.HexCoordinate(firstHitInfo.transform.position));
            result.LandingCell = LandingCell(hitCell, firstHitInfo.point);

            return result;
        }
        
        private HexCell LandingCell(HexCell fromCell, Vector2 hitPoint)
        {
            Vector2 hitDirection = (hitPoint - _grid.CellPosition(fromCell.Coordinates)).normalized;
            float hitAngle = Vector2.SignedAngle(Vector2.left, hitDirection) + 180f;
            int segment = Mathf.RoundToInt(hitAngle / 60f);

            HexCell[] neighbourCells = _grid.NeighbourCells(fromCell);
            if (neighbourCells[segment] == null) return null;

            return neighbourCells[segment];
        }
        
        private int Layer(RaycastHit2D hitInfo)
        {
            return hitInfo.transform.gameObject.layer;
        }
        
        private LayerMask BounceLayerMask(int hitLayer)
        {
            int oppositeWallLayer = hitLayer == _leftWallLayer ? _rightWallLayer : _leftWallLayer;
            LayerMask bounceMask = 1 << oppositeWallLayer | 1 << _topWallLayer | 1 << _cellLayer;
            return bounceMask;
        }

        private Vector2 BounceDirection(Vector2 direction)
        {
            return new Vector2(-direction.x, direction.y);
        }
    }

    public struct ShootRaycastResult
    {
        public HexCell LandingCell;
        public List<Vector2> HitPoints;
    }
}