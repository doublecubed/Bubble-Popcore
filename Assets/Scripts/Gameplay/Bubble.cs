// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        private BubblePool _pool;
        [SerializeField] private TextMeshPro valueText;
        
        public HexCell Cell { get; private set; }
        public int Value { get; private set; }

        public void SetPool(BubblePool pool)
        {
            _pool = pool;
        }
        
        
        public void Initialize(HexCell cell)
        {
            Cell = cell;
            Cell.Bubble = this;
            Value = cell.Value;
            valueText.text = GameVar.Value(Value).ToString("F00");
        }

        public void Blank()
        {
            Cell = null;
        }

        public void Pop()
        {
            Cell.Bubble = null;
            Cell.Value = 0;
            _pool.Recall(this);
        }

        public void Drop()
        {
            Pop();
        }
    }   
}