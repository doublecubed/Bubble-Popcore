// Onur Ereren - June 2024
// Popcore case

// This script provides the global values throughout the game.

using UnityEngine;

namespace PopsBubble
{
    public static class GameVar
    {
        // Gameplay variables
        private const int BaseValue = 2;

        public const float CellWidth = 1f;

        public static float RowHeight()
        {
            return CellWidth * 0.5f * Mathf.Sqrt(3);
        }
        
        public static int DisplayValue(int power)
        {
            return (int)Mathf.Pow(BaseValue, power);
        }

       
        // Animation and Tweening variables

        public const float BubbleAppearDuration = 0.2f;

        public const float BubbleMergeDuration = 0.2f;

        public const float BubbleValueSwitchDuration = 0.2f;

        public const float GridDropDuration = 0.2f;

        public const float BubbleTrailMoveSpeed = 30f;
    }
}