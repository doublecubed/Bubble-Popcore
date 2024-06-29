using System;
using System.Collections;
using System.Collections.Generic;
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

        public GameObject _currentGhost;
        public float _hitAngle;
        public int _hitSegment;
        
        private HexCell _hitCell;
        private HexCell _fromCell;

        private Vector2 _gizmoStart;
        private Vector2 _gizmoEnd;
        
        private void Start()
        {
            _gameFlow = FindObjectOfType<GameFlow>();
            _input = GetComponent<PlayerInput>();
            _drawer = GetComponent<IPathDrawing>();
        }

        private void Update()
        {
            if (_gameFlow.GameIsRunning) CastTheFirstRay();   
        }

        private void CastTheFirstRay()
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
            _gizmoStart = _grid.CellPosition(fromCell.Coordinates);
            _gizmoEnd = hitPoint;
            
            Vector2 hitDirection = (hitPoint - _grid.CellPosition(fromCell.Coordinates)).normalized;
            float hitAngle = Vector2.SignedAngle(Vector2.left, hitDirection) + 180;
            _hitAngle = hitAngle;
            int segment = Mathf.RoundToInt(hitAngle / 60);
            _hitSegment = segment;

            HexCell[] neighbourCells = _grid.NeighbourCells(fromCell);
            if (neighbourCells[segment] == null) return;
            
            _currentGhost.transform.position = _grid.CellPosition(_grid.NeighbourCells(fromCell)[segment].Coordinates);

        }


        private void OnDrawGizmos()
        {
            if (_gizmoStart == Vector2.zero && _gizmoEnd == Vector2.zero) return;
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(_gizmoStart, _gizmoEnd);
            
        }
    }
}