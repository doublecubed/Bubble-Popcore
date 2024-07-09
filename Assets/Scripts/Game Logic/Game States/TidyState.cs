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
        private GameFlow _gameFlow;
        private HexGrid _grid;
        private float _rowHeight;
        private CancellationToken _ct;

        private int _dropdownFrequency;
        
        public TidyState()
        {
            _gameFlow = DependencyContainer.GameFlow;
            _grid = DependencyContainer.Grid;
            _rowHeight = GameVar.RowHeight();
            _ct = new CancellationToken();

            _dropdownFrequency = _gameFlow.LevelProfile.DropdownFrequency;
        }
        

        public override async void OnEnter()
        {
            int moveDown = Random.Range(0, _dropdownFrequency);
            
            if (moveDown == 0) await _grid.MoveGridDown();

            OnStateComplete?.Invoke();
        }
    }
}
