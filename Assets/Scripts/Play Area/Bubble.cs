// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.Serialization;

namespace PopsBubble
{
    public class Bubble : MonoBehaviour
    {
        private BubblePool _pool;
        private HexGrid _grid;
        private float _dropDuration;

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private TextMeshPro _valueText;

        [SerializeField] private Color _defaultTextColor;

        [SerializeField] private int _deafultSortOrder;
        
        public void SetReferences(BubblePool pool, HexGrid grid)
        {
            _pool = pool;
            _grid = grid;
            _dropDuration = GameVar.GridDropDuration;
        }
        
        
        public void Initialize(Color color, int value)
        {
            _renderer.sortingOrder = _deafultSortOrder;
            _valueText.sortingOrder = _deafultSortOrder + 1;
            
            _valueText.color = _defaultTextColor;
            _valueText.text = GameVar.DisplayValue(value).ToString("F00");
        }

        public void SwitchValue(int value)
        {
            _valueText.DOText(GameVar.DisplayValue(value).ToString("F00"), 0.1f);
        }
        
        public void SendToBack()
        {
            _renderer.sortingOrder = 0;
            _valueText.sortingOrder = 1;
            _valueText.DOColor(new Color(0f, 0f, 0f, 0f), 0.1f);
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