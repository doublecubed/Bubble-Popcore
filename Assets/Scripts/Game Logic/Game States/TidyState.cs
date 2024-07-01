// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

namespace PopsBubble
{
    public class TidyState : GameState
    {
        private Transform _gridTransform;
        private float _rowHeight;
        
        public TidyState()
        {
            _gridTransform = DependencyContainer.Grid.transform;
            _rowHeight = GameVar.RowHeight();
        }
        

        public override void OnEnter()
        {
            Vector2 _currentPos = _gridTransform.position;
            _currentPos += Vector2.down * _rowHeight;
            _gridTransform.position = _currentPos;
            
            OnStateComplete?.Invoke();
        }
    }
}
