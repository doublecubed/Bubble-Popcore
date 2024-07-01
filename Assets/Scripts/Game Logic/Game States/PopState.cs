// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class PopState : GameState
    {
        private BubbleRaycaster _raycaster;

        public PopState()
        {
            _raycaster = DependencyContainer.BubbleRaycaster;
        }
        

        public override void OnEnter()
        {
            Debug.Log("Entering pop state");
            _raycaster.CalculatePop();

            OnStateComplete?.Invoke();
        }
        
    }
}
