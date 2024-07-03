// Onur Ereren - June 2024
// Popcore case

using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace PopsBubble
{
    public class ShootState : GameState
    {
        #region REFERENCES
        
        private Transform _ghostBubble;
        
        private PlayerInput _input;
        private IPathDrawer _pathDrawer;
        private IRaycaster _shootRaycaster;
        private BubbleRaycaster _raycaster;
        
        #endregion
        
        #region VARIABLES

        private HexCell _previousFrameTarget;
        private Tween _ghostBubbleTween;
        
        #endregion
        
        #region CONSTRUCTOR
        
        public ShootState()
        {
            _ghostBubble = DependencyContainer.GhostBubble;
            
            _input = DependencyContainer.PlayerInput;
            _pathDrawer = DependencyContainer.PathDrawer;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _raycaster = DependencyContainer.BubbleRaycaster;
            _raycaster.OnBubbleShot += ShotIsTaken;
        }
        
        #endregion
        
        #region METHODS
        
        #region State Operation
        
        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            UpdateShootingPath();

        }

        public override void OnExit()
        {
            _pathDrawer.ClearPath();
            ClearGhostBubble();
        }

        #endregion
        
        #region Internal
        
        private void ShotIsTaken()
        {
            OnStateComplete?.Invoke();
        }

        private void UpdateShootingPath()
        {
            ShootRaycastResult raycastResult = _shootRaycaster.ShootRaycast(_input.InputVector);
            HandlePathDrawing(raycastResult.HitPoints);
            HandleGhostBubble(raycastResult);

            _previousFrameTarget = raycastResult.LandingCell;
            _raycaster._targetCell = raycastResult.LandingCell;
        }

        private void HandleGhostBubble(ShootRaycastResult raycastResult)
        {
            if (raycastResult.HitPoints.Count == 0)
            {
                ClearGhostBubble();
                return;
            }
            
            if (_previousFrameTarget != raycastResult.LandingCell) 
                PositionGhostBubble(raycastResult.LandingCell.Position);
        }
        
        private void HandlePathDrawing(List<Vector2> points)
        {
            if (points.Count == 0)
            {
                _pathDrawer.ClearPath();
                return;
            }
            
            _pathDrawer.DrawPath(points);
        }

        private void PositionGhostBubble(Vector2 position)
        {
            _ghostBubbleTween.Kill();
            _ghostBubble.position = position;
            _ghostBubble.localScale = Vector2.zero;
            _ghostBubbleTween = _ghostBubble.DOScale(Vector2.one, GameVar.BubbleAppearDuration);
        }

        private void ClearGhostBubble()
        {
            _ghostBubbleTween.Kill();
            _ghostBubble.localScale = Vector2.zero;
        }
        
        #endregion
        
        #endregion
    }
}