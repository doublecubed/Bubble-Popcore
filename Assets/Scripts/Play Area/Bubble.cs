using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        [SerializeField] private TextMeshPro valueText;
        
        public HexCell Cell { get; private set; }
        public int Value { get; private set; }

        public void Initialize(HexCell cell)
        {
            Cell = cell;
            Cell.Bubble = this;
            Value = cell.Value;
            valueText.text = Mathf.Pow(2, Value).ToString("F00");
        }

        public void Pop()
        {
            Cell.Bubble = null;
            Cell.Value = 0;
            Destroy(gameObject);
        }
        
    }   
}