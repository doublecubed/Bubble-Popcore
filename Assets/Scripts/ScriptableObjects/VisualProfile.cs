// Onur Ereren - July 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace PopsBubble
{   
    [CreateAssetMenu(fileName = "New Visual Profile", menuName = "PopsBubble/Visual Profile")]
    public class VisualProfile : ScriptableObject
    {
        public Color[] BubbleColors;
    }
}
