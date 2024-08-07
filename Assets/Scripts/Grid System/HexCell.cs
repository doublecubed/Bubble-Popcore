// Onur Ereren - June 2024
// Popcore case

// HexCell used by the HexGrid.


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
                Bubble = await _pool.Dispense(this);
            }
        }

        // Clears only the value. For moving grid up and down, since new data will be written into it.
        public void ClearValue()
        {
            Value = 0;
            Bubble = null;
            _hexCollider.enabled = false;
        }
        
        // Clear the hex, and decide what to do with the bubble
        public void Clear(bool recallBubble = false)
        {
            Value = 0;
            _hexCollider.enabled = false;
            if (Bubble == null) return;

            if (recallBubble)
            {
                Bubble.PlayParticles();
                _pool.Recall(Bubble);
            }
            else
            {
                Bubble.Pop();
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
            Bubble = fromCell.Bubble == null ? null : fromCell.Bubble;
            //Bubble = fromCell.Value == 0 ? null : fromCell.Bubble;
            _hexCollider.enabled = fromCell.Value != 0;
        }
        
        // Assigning data directly, used for scrambling
        public void AssignData(ScrambleData scrambleData)
        {
            Value = scrambleData.Value;
            Bubble = scrambleData.Bubble;
        }

        // This is used to transfer the bubble from mover to the hex
        public void TransferBubbleAndUpdate(Bubble bubble, int value)
        {
            Bubble = bubble;
            bubble.transform.parent = _grid.BubbleParent;
            bubble.transform.position = _grid.CellPosition(this);
            _hexCollider.enabled = true;
            Value = value;
        }

        public void UpdatePosition()
        {
            Position = _grid.CellPosition(Coordinates);
            transform.position = Position;
        }
        
        public async UniTask MoveBubble()
        {
            if (Bubble != null) 
                await Bubble.transform.DOMove(Position, GameVar.GridDropDuration).WithCancellation(_ct);
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