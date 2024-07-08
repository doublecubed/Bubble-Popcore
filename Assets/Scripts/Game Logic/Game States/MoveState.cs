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

        private HexGrid _grid;
        private IShootIndicator _shootIndicator;
        private IPathMover _pathMover;
        private IRaycaster _shootRaycaster;
        private IShootValueCalculator _shootCalculator;

        private HexCell _targetHexCell;
        
        public MoveState()
        {
            _moverTransform = DependencyContainer.MoverTrail;

            _grid = DependencyContainer.Grid;
            _shootIndicator = DependencyContainer.ShootIndicator;
            _pathMover = DependencyContainer.PathMover;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
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
            
            SetLastWaypointToCell(waypoints);
            
            await _pathMover.MoveOnPath(waypoints);
            
            _targetHexCell.TransferBubbleAndUpdate(shootingBubble, _shootCalculator.GetValue());
            
            //await GenerateBubble(_targetHexCell);
            
            OnStateComplete?.Invoke();
        }


        private void SetLastWaypointToCell(List<Vector2> waypoints)
        {
            Vector2Int finalCellCoordinates = _grid.HexCoordinate(waypoints[^1]);
            waypoints[^1] = _grid.CellPosition(finalCellCoordinates);
        }
     
        
    }
}
