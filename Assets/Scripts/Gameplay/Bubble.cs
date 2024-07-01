// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        private BubblePool _pool;
        private HexGrid _grid;
        private float _dropDuration;
        
        [SerializeField] private TextMeshPro valueText;
        
        public HexCell Cell { get; private set; }
        public int Value { get; private set; }

        public void SetReferences(BubblePool pool, HexGrid grid)
        {
            _pool = pool;
            _grid = grid;
            _dropDuration = GameVar.GridDropDuration;
        }
        
        
        public void Initialize(HexCell cell)
        {
            Cell = cell;
            Cell.Bubble = this;
            Value = cell.Value;
            valueText.text = GameVar.DisplayValue(Value).ToString("F00");
        }

        public void Blank()
        {
            Cell = null;
        }

        public async UniTask DropDown(HexCell newCell)
        {
            Cell = newCell;
            Value = newCell.Value;
            newCell.Bubble = this;

            await transform.DOMove(_grid.CellPosition(Cell), _dropDuration);
        }
        
        public void Pop()
        {
            Cell.Bubble = null;
            Cell.Value = 0;
            _pool.Recall(this);
        }

        public void Detach()
        {
            Pop();
        }
    }   
}