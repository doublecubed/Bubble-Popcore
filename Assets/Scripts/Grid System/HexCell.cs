// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace PopsBubble
{
    public class HexCell : MonoBehaviour
    {
        #region REFERENCES
        
        private HexGrid _grid;
        private BubblePool _pool;
        private PolygonCollider2D _hexCollider;

        #endregion
        
        #region VARIABLES
        
        public Vector2Int Coordinates { get; private set; }
        public Vector2 Position { get; private set; }
        public int Value { get; set; }
        public Bubble Bubble { get; set; }

        private CancellationToken _ct;
        
        #endregion
        
        #region METHODS
        
        #region Initialization
        
        public void Initialize(HexGrid grid, Vector2Int coords, Vector2 pos)
        {
            _grid = grid;
            _pool = DependencyContainer.BubblePool;
            _hexCollider = GetComponent<PolygonCollider2D>();

            _ct = this.GetCancellationTokenOnDestroy();
            
            Coordinates = coords;
            Position = pos;
        }

        #endregion
        
        #region Data Manipulation
        
        // This is to be used for level start, and top row Bubble creation.
        public async UniTask SetData(int value)
        {
            Value = value;
            if (value != 0)
            {
                _hexCollider.enabled = true;
                await _pool.Dispense(this);
            }
        }

        // This is for immediate clearing. Used in dropdown, to clear the top row before writing in new data from above
        public void Clear()
        {
            Value = 0;
            _hexCollider.enabled = false;
            Bubble = null;
        }
        
        // Transferring data before dropdown
        public void TransferData(HexCell fromCell)
        {
            Value = fromCell.Value;
            Bubble = fromCell.Value == 0 ? null : fromCell.Bubble;
            _hexCollider.enabled = fromCell.Value != 0;
        }
        
        public async UniTask UpdateForDropDown()
        {
            Position = _grid.CellPosition(Coordinates);
            transform.position = Position;
            if (Bubble != null)
            {
                await Bubble.transform.DOMove(Position, GameVar.GridDropDuration).WithCancellation(_ct);
            }
        }
        
        #endregion
        
        #region Bubble Manipulation
        
        // This is when a bubble is popped or dropped
        public async UniTask PopBubble()
        {
            _hexCollider.enabled = false;
            Bubble.Pop();
            Bubble = null;
        }

        public async UniTask DropBubble()
        {
            
        }
        
        #endregion
        
        #endregion
    }
}