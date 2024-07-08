// Onur Ereren - June 2024
// Popcore case

// This script provides the global values throughout the game.

using UnityEngine;

namespace PopsBubble
{
    public static class GameVar
    {
        #region GAMEPLAY
        
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
        
        #endregion

        #region ANIMATION AND TWEENING
       
        public const float BubbleAppearDuration = 0.2f;

        public const float BubbleMergeDuration = 0.2f;
        
        public const float BubbleValueSwitchDuration = 0.2f;
        public const float GridDropDuration = 0.2f;
        public const float BubbleTrailMoveSpeed = 30f;

        public const float NeighbourKnockbackDistance = 0.15f;
        // This duration used for both knockback and recover, effective duration is double the value below
        public const float NeighbourKnockbackDuration = 0.075f; 
        
        #endregion
        
        #region COLOUR AND SORT ORDER

        public const float GhostBubbleAlpha = 0.4f;

        public const int FrontMostSortOrder = 499; //Bubble particles is 500, must be lower than that.
        
        #endregion
        
        // Bubble Pop Variables

        public const float BubblePopLateralVectorMin = 0.4f;
        public const float BubblePopLateralVectorMax = 1f;
        public const float BubblePopVerticalVectorMin = 0.1f;
        public const float BubblePopVerticalVectorMax = 1f;

        public const float BubblePopForce = 100f;
    }
}