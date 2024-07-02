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
        private HexGrid _grid;
        private float _rowHeight;
        private CancellationToken _ct;
        
        public TidyState()
        {
            _grid = DependencyContainer.Grid;
            _rowHeight = GameVar.RowHeight();
            _ct = new CancellationToken();
        }
        

        public override async void OnEnter()
        {
            // int moveDown = Random.Range(0, 3);
            //
            // if (moveDown == 0) await _grid.MoveGridDown();

            OnStateComplete?.Invoke();
        }
    }
}
