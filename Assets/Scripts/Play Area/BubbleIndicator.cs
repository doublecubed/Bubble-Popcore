// Onur Ereren - June 2024
// Popcore case

using UnityEngine;
using TMPro;

namespace PopsBubble
{
    public class BubbleIndicator : MonoBehaviour, IShootIndicator
    {
        [SerializeField] private TextMeshPro _shootIndicator;
        [SerializeField] private TextMeshPro _nextIndicator;
        
        public void Set(int value, int nextValue)
        {
            _shootIndicator.text = GameVar.DisplayValue(value).ToString();
            _nextIndicator.text = GameVar.DisplayValue(nextValue).ToString();
        }
    }
}
