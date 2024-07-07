// Onur Ereren - June 2024
// Popcore case

using System;
using UnityEngine;
using TMPro;

namespace PopsBubble
{
    public class BubbleIndicator : MonoBehaviour, IShootIndicator
    {
        private GameFlow _gameFlow;

        [SerializeField] private SpriteRenderer _shootRenderer;
        [SerializeField] private SpriteRenderer _nextRenderer;
        
        [SerializeField] private TextMeshPro _shootIndicator;
        [SerializeField] private TextMeshPro _nextIndicator;

        private void Start()
        {
            _gameFlow = DependencyContainer.GameFlow;
        }

        public void Set(int value, int nextValue)
        {
            _shootRenderer.color = _gameFlow.ColorByValue(value);
            _nextRenderer.color = _gameFlow.ColorByValue(nextValue);
            
            _shootIndicator.text = GameVar.DisplayValue(value).ToString();
            _nextIndicator.text = GameVar.DisplayValue(nextValue).ToString();
        }

    }
}
