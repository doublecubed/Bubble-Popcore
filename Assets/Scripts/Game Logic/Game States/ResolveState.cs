// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class ResolveState : GameState
    {
        private BubbleRaycaster _raycaster;

        public ResolveState()
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
