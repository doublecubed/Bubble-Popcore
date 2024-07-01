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
        

        public override async void OnEnter()
        {
            await _raycaster.CalculatePop();
            await _raycaster.CalculateIslands();
            
            OnStateComplete?.Invoke();
        }
        
    }
}
