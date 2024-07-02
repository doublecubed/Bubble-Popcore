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
        
        public void SetReferences(BubblePool pool, HexGrid grid)
        {
            _pool = pool;
            _grid = grid;
            _dropDuration = GameVar.GridDropDuration;
        }
        
        
        public void Initialize(Color color, int value)
        {
            valueText.text = GameVar.DisplayValue(value).ToString("F00");
        }

        public void Pop()
        {
            _pool.Recall(this);
        }

        public void Detach()
        {
            Pop();
        }
    }   
}