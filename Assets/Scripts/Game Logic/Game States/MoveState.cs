// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class MoveState : GameState
    {
        private BubbleRaycaster _raycaster;

        public MoveState()
        {
            _raycaster = DependencyContainer.BubbleRaycaster;
        }
        
        public override void OnEnter()
        {
            Debug.Log("Entering move state");
            _raycaster.MakeNewBubble();

            OnStateComplete?.Invoke();
        }
        
        
    }
}
