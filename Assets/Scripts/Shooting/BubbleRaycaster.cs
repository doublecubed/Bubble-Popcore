// Onur Ereren - June 2024
// Popcore case

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PopsBubble
{
    public class BubbleRaycaster : MonoBehaviour
    {
        
        [SerializeField] private Transform _shootingPoint;
        private GameFlow _gameFlow;
        private PlayerInput _input;
        [SerializeField] private HexGrid _grid;
        
        private IPathDrawing _drawer;

        [SerializeField] private GameObject _bubbleGhostPrefab;
        [SerializeField] private GameObject _bubblePrefab;
        
        public GameObject _currentGhost;
        public float _hitAngle;
        public int _hitSegment;
        
        private HexCell _hitCell;
        private HexCell _fromCell;

        private bool _isShooting;

        public Transform _ghostRestingPoint;
        public HexCell _targetCell;

        public IShootValueCalculator _shootCalculator;
        
        public Action OnBubbleShot;
        
        private void Start()
        {
            _gameFlow = DependencyContainer.GameFlow;
            _input = GetComponent<PlayerInput>();
            _drawer = GetComponent<IPathDrawing>();
            _shootCalculator = DependencyContainer.ShootCalculator;
            
            _input.OnMouseButtonUp += ShootSignal;
        }

        public void CastTheFirstRay()
        {
            Vector2 previousDirection = _input.InputVector;

            float directionAngle = Vector2.Angle(previousDirection, Vector2.right);
            if (directionAngle < 10 || directionAngle > 170)
            {
                _drawer.ClearPath();
                return;
            }
            
            if (previousDirection == Vector2.zero)
            {
                _drawer.ClearPath();
                return;
            }
            
            RaycastHit2D previousHitInfo = Physics2D.Raycast(_shootingPoint.position, previousDirection);
            List<Vector2> hitPoints = new List<Vector2> { previousHitInfo.point };

            int hitLayer = previousHitInfo.transform.gameObject.layer;

            if (HitsTopBoundary(previousHitInfo))
            {
                
            }
            
            while (hitLayer == 6 || hitLayer == 7)
            {
                Vector2 bounceDirection = new Vector2(-previousDirection.x, previousDirection.y);
                int otherWallLayer = hitLayer == 6 ? 7 : 6;
                LayerMask bounceMask = 1 << otherWallLayer | 1 << 8 | 1 << 9;

                RaycastHit2D nextHitInfo =
                    Physics2D.Raycast(previousHitInfo.point, bounceDirection, Mathf.Infinity, bounceMask);
                
                hitPoints.Add(nextHitInfo.point);

                previousHitInfo = nextHitInfo;
                hitLayer = nextHitInfo.transform.gameObject.layer;
                previousDirection = bounceDirection;
            }

            _drawer.DrawPath(hitPoints);

            _hitCell = previousHitInfo.transform.GetComponent<Bubble>().Cell;
            ShowTargetCell(_hitCell, previousHitInfo.point);
        }

        private void ShowTargetCell(HexCell fromCell, Vector2 hitPoint)
        {
            
            Vector2 hitDirection = (hitPoint - _grid.CellPosition(fromCell.Coordinates)).normalized;
            float hitAngle = Vector2.SignedAngle(Vector2.left, hitDirection) + 180;
            _hitAngle = hitAngle;
            int segment = Mathf.RoundToInt(hitAngle / 60);
            _hitSegment = segment;

            HexCell[] neighbourCells = _grid.NeighbourCells(fromCell);
            if (neighbourCells[segment] == null) return;

            _targetCell = neighbourCells[segment];
            
            _currentGhost.transform.position = _grid.CellPosition(_targetCell.Coordinates);

        }

        public void ResetGhost()
        {
            _currentGhost.transform.position = _ghostRestingPoint.position;
        }

        public void ResetTargetCell()
        {
            _targetCell = null;
        }
        
        private void ShootSignal()
        {
            if (!(_gameFlow.CurrentGameState() is ShootState)) return; 
            
            ResetGhost();
            Debug.Log("Shoot!");

            _drawer.ClearPath();
            OnBubbleShot?.Invoke();
        }

        public void MakeNewBubble()
        {
            if (_targetCell == null)
            {
                Debug.Log("target cell is null");
                return;
            }
            _targetCell.Value = _shootCalculator.GetValue();
            
            GameObject newBubble = Instantiate(_bubblePrefab, _grid.CellPosition(_targetCell.Coordinates),
                Quaternion.identity, _grid.transform);
            newBubble.GetComponent<Bubble>().Initialize(_targetCell);
            

        }

        public void CalculatePop()
        {
            /*
            while (_targetCell != null)
            {
                CellSearchResult initialSearchResult = _grid.IterateForValue(_targetCell);
                int chainLength = initialSearchResult.ValueCells.Count;
                int nextValue = _targetCell.Value * chainLength;

                if (initialSearchResult.ValueCells.Count <= 1) break;
                
                for (int i = 0; i < initialSearchResult.ValueCells.Count; i++)
                {
                    initialSearchResult.ValueCells[i].Bubble.Pop();
                }
                
                HexCell nextChainHead = initialSearchResult.NeighbourCells[0];
                int nextChainLength = 0;
                for (int i = 0; i < initialSearchResult.NeighbourCells.Count; i++)
                {
                    if (initialSearchResult.NeighbourCells[i].Value != nextValue) continue;

                    CellSearchResult nextSearchResult = _grid.IterateForValue(initialSearchResult.NeighbourCells[i]);
                    if (nextSearchResult.ValueCells.Count > nextChainLength)
                    {
                        nextChainLength = nextSearchResult.ValueCells.Count;
                        nextChainHead = initialSearchResult.NeighbourCells[i];
                    }
                }

                HexCell spawnCell = initialSearchResult.ValueCells[0];
                HexCell[] reverseNeighbours = _grid.NeighbourCells(nextChainHead);

                for (int i = 0; i < 6; i++)
                {
                    if (reverseNeighbours[i] != null && initialSearchResult.ValueCells.Contains(reverseNeighbours[i])
                    spawnCell = reverseNeighbours[i];
                }
                
                

            }
            */
            
            // Scan from the first cell
            CellSearchResult cellSearchResult = _grid.IterateForValue(_targetCell);

            // Pop the cells that match the value
            for (int i = 0; i < cellSearchResult.ValueCells.Count; i++)
            {
                cellSearchResult.ValueCells[i].Bubble.Pop();
            }

            // Calculate the new value
            int newValue = _targetCell.Value * cellSearchResult.ValueCells.Count;

            // Search all the neighbours. Pass the ones that don't match the new value

            
            
            
            
            
            ResetTargetCell();
        }
        
        private bool HitsTopBoundary(RaycastHit2D hitInfo)
        {
            return hitInfo.transform.gameObject.layer == 8;
        }
    }
}