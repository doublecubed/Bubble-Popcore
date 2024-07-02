// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class MoveState : GameState
    {
        private IPathMover _mover;
        private BubbleRaycaster _raycaster;

        public MoveState()
        {
            _mover = DependencyContainer.PathMover;
            _raycaster = DependencyContainer.BubbleRaycaster;
        }
        
        public override async void OnEnter()
        {
            
            
            _raycaster.MakeNewBubble();

            OnStateComplete?.Invoke();
        }
        
        
    }
}
