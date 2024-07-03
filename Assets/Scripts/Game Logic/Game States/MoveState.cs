// Onur Ereren - June 2024
// Popcore case

using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace PopsBubble
{
    public class MoveState : GameState
    {
        private Transform _moverTransform;
        
        private IPathMover _pathMover;
        private IRaycaster _shootRaycaster;
        private IShootValueCalculator _shootCalculator;
        private BubbleRaycaster _raycaster;

        private HexCell _targetHexCell;
        
        public MoveState()
        {
            _moverTransform = DependencyContainer.MoverTrail;
            
            _pathMover = DependencyContainer.PathMover;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _raycaster = DependencyContainer.BubbleRaycaster;
            _shootCalculator = DependencyContainer.ShootCalculator;
        }
        
        public override async void OnEnter()
        {
            _targetHexCell = _shootRaycaster.ShootResult().LandingCell;

            List<Vector2> waypoints = _shootRaycaster.ShootResult().HitPoints;

            await _pathMover.MoveOnPath(_moverTransform, waypoints);
            
            await GenerateBubble(_targetHexCell);
            
            OnStateComplete?.Invoke();
        }

        private async UniTask GenerateBubble(HexCell targetCell)
        {
            if (targetCell == null) return;
            
            await targetCell.SetStartingData(_shootCalculator.GetValue());
        }
        
        
    }
}
