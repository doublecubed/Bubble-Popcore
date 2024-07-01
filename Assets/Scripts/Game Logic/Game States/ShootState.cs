// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class ShootState : GameState
    {
        private BubbleRaycaster _raycaster;
        
        public ShootState()
        {
            _raycaster = DependencyContainer.BubbleRaycaster;
            _raycaster.OnBubbleShot += ShotIsTaken;
        }
        
        public override void OnEnter()
        {
            Debug.Log("Entering shoot state");
        }

        public override void OnUpdate()
        {
            _raycaster.CastTheFirstRay();
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