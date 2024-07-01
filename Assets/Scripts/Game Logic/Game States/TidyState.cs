// Onur Ereren - June 2024
// Popcore case

using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;

namespace PopsBubble
{
    public class TidyState : GameState
    {
        private Transform _gridTransform;
        private float _rowHeight;
        private CancellationToken _ct;
        
        public TidyState()
        {
            _gridTransform = DependencyContainer.Grid.transform;
            _rowHeight = GameVar.RowHeight();
            _ct = new CancellationToken();
        }
        

        public override async void OnEnter()
        {


            OnStateComplete?.Invoke();
        }
    }
}
