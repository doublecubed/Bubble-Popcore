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
        public async UniTask SetStartingData(int value)
        {
            Value = value;
            if (value != 0)
            {
                _hexCollider.enabled = true;
                await _pool.Dispense(this);
            }
        }

        // This is for immediate clearing. Used in dropdown and move up, to clear the row before writing in new data
        public void Clear(bool recallBubble = false)
        {
            Value = 0;
            _hexCollider.enabled = false;
            if (Bubble != null && recallBubble)
            {
                _pool.Recall(Bubble);
            }
            Bubble = null;
        }

        // Used during merging
        public void SwitchValue(int newValue)
        {
            Value = newValue;
        }
        
        // Transferring data before row movement
        public void TransferData(HexCell fromCell)
        {
            Value = fromCell.Value;
            Bubble = fromCell.Value == 0 ? null : fromCell.Bubble;
            _hexCollider.enabled = fromCell.Value != 0;
        }
        
        // Assigning data directly, used for scrambling
        public void AssignData(ScrambleData scrambleData)
        {
            Value = scrambleData.Value;
            Bubble = scrambleData.Bubble;
        }
        
        public async UniTask UpdateAndMove()
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

        public async UniTask DetachForMerge()
        {
            Bubble = null;
        }
        
        public async UniTask DetachForDrop()
        {
            await PopBubble(); // Currently the same functionality
        }
        
        #endregion
        
        #endregion
    }

    public struct ScrambleData
    {
        public int Value;
        public Bubble Bubble;
    }
}