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

        private IShootIndicator _shootIndicator;
        private IPathMover _pathMover;
        private IRaycaster _shootRaycaster;
        private IShootValueCalculator _shootCalculator;
        private BubbleRaycaster _raycaster;

        private HexCell _targetHexCell;
        
        public MoveState()
        {
            _moverTransform = DependencyContainer.MoverTrail;

            _shootIndicator = DependencyContainer.ShootIndicator;
            _pathMover = DependencyContainer.PathMover;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _raycaster = DependencyContainer.BubbleRaycaster;
            _shootCalculator = DependencyContainer.ShootCalculator;
        }
        
        public override async void OnEnter()
        {
            _pathMover.ResetPosition();
            _targetHexCell = _shootRaycaster.ShootResult().LandingCell;

            Bubble shootingBubble = _shootIndicator.CurrentBubble();
            shootingBubble.transform.parent = _moverTransform;
            shootingBubble.transform.SetSiblingIndex(0);
            
            List<Vector2> waypoints = _shootRaycaster.ShootResult().HitPoints;
            
            
            
            await _pathMover.MoveOnPath(waypoints);
            
            _targetHexCell.TransferBubbleAndUpdate(shootingBubble, _shootCalculator.GetValue());
            
            //await GenerateBubble(_targetHexCell);
            
            OnStateComplete?.Invoke();
        }


        private void SetLastWaypointToCell()
        {
            
        }
     
        
    }
}
