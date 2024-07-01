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
            _shootIndicator.text = GameVar.Value(value).ToString();
            _nextIndicator.text = GameVar.Value(nextValue).ToString();
        }
    }
}
