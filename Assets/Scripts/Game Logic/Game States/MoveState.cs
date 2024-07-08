// Onur Ereren - June 2024
// Popcore case

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;

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

        private CancellationToken _ct;
        
        public MoveState()
        {
            _moverTransform = DependencyContainer.MoverTrail;

            _grid = DependencyContainer.Grid;
            _shootIndicator = DependencyContainer.ShootIndicator;
            _pathMover = DependencyContainer.PathMover;
            _shootRaycaster = DependencyContainer.ShootRaycaster;
            _shootCalculator = DependencyContainer.ShootCalculator;

            _ct = new CancellationToken();
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
            
            await KnockbackNeighbourCells(_targetHexCell);
            
            OnStateComplete?.Invoke();
        }

        private async UniTask KnockbackNeighbourCells(HexCell targetCell)
        {
            List<HexCell> liveNeighbours =
                _grid.NeighbourCells(targetCell).Where(cell => cell != null && cell.Value != 0).ToList();

            List<UniTask> knockbackTasks = new List<UniTask>();
            foreach (HexCell cell in liveNeighbours)
            {
                Vector2 knockbackDirection = (cell.Position - targetCell.Position).normalized;
                Vector2 knockbackPosition = cell.Position + knockbackDirection * GameVar.NeighbourKnockbackDistance;
                knockbackTasks.Add(cell.Bubble.transform.DOMove(knockbackPosition, GameVar.NeighbourKnockbackDuration)
                    .OnComplete(() =>
                    {
                        cell.Bubble.transform.DOMove(cell.Position, GameVar.NeighbourKnockbackDuration);
                    }).WithCancellation(_ct));
            }

            await UniTask.WhenAll(knockbackTasks);
        }

        private void SetLastWaypointToCell(List<Vector2> waypoints)
        {
            Vector2Int finalCellCoordinates = _grid.HexCoordinate(waypoints[^1]);
            waypoints[^1] = _grid.CellPosition(finalCellCoordinates);
        }
     
        
    }
}
