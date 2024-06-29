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

        private IPathDrawing _drawer;

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
            Vector2 direction = _input.InputVector;

            if (direction == Vector2.zero)
            {
                _drawer.ClearPath();
                return;
            }
            
            RaycastHit2D hitInfo = Physics2D.Raycast(_shootingPoint.position, direction);

            List<Vector2> hitPoints = new List<Vector2> { hitInfo.point };

            int hitLayer = hitInfo.transform.gameObject.layer;
            if (hitLayer is 6 or 7)
            {
                Vector2 bounceDirection = new Vector2(-direction.x, direction.y);
                int otherWallLayer = hitLayer == 6 ? 7 : 6;
                LayerMask bounceMask = 1 << otherWallLayer | 1 << 8 | 1 << 9;

                RaycastHit2D secondHitInfo =
                    Physics2D.Raycast(hitInfo.point, bounceDirection, Mathf.Infinity, bounceMask);

                Debug.Log($"second hit is {secondHitInfo.transform.name}");
                
                hitPoints.Add(secondHitInfo.point);
            }

            _drawer.DrawPath(hitPoints);
        }

    }
}