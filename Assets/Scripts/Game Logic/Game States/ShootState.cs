// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class ShootState : GameState
    {
        private PlayerInput _input;
        private IPathDrawer _pathDrawer;
        private IRaycaster _shootRaycaster;
        private BubbleRaycaster _raycaster;
        
        public ShootState()
        {
            _input = DependencyContainer.PlayerInput;
            _pathDrawer = DependencyContainer.PathDrawer;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _raycaster = DependencyContainer.BubbleRaycaster;
            _raycaster.OnBubbleShot += ShotIsTaken;
        }
        
        public override void OnEnter()
        {
        }

        public override void OnUpdate()
        {
            ShootRaycastResult raycastResult = _shootRaycaster.ShootRaycast(_input.InputVector);
            _pathDrawer.DrawPath(raycastResult.HitPoints);
            //_raycaster.CastTheFirstRay();
        }

        public override void OnExit()
        {
            
        }

        private void ShotIsTaken()
        {
            OnStateComplete?.Invoke();
        }
    }

}