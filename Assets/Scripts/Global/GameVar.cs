// Onur Ereren - June 2024
// Popcore case

// This script provides the global values throughout the game.

using UnityEngine;

namespace PopsBubble
{
    public static class GameVar
    {
        #region GAMEPLAY
        
        private const int PowerBaseValue = 2;

        public const int MaximumPowerValue = 11;
        
        public const float CellWidth = 1f;

        public static float RowHeight()
        {
            return CellWidth * 0.5f * Mathf.Sqrt(3);
        }
        
        public static int DisplayValue(int power)
        {
            return (int)Mathf.Pow(PowerBaseValue, power);
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

        public const float MergePointRiseDistance = 1.5f;
        public const float MergePointRiseDuration = 1f;
        public const float MergePointRiseScaleIncrease = 1.2f;

        public const float MaxValueCellPopScale = 1.2f;
        public const float MaxValueCellPopDuration = 0.5f;
        
        #endregion
        
        #region COLOUR AND SORT ORDER

        public const float GhostBubbleAlpha = 0.4f;

        public const int FrontMostSortOrder = 499; //Bubble particles is 500, must be lower than that.

        public const string BackgroundSortingLayer = "Background";
        public const string ShadowSortingLayer = "Shadow";
        public const string MergingSortingLayer = "Merging";
        public const string DefaultSortingLayer = "Default";
        public const string FallingSortingLayer = "Falling";
        public const string ParticleSortingLayer = "Particle";

        #endregion
        
        #region BUBBLE POP

        public const float BubblePopLateralVectorMin = 0.4f;
        public const float BubblePopLateralVectorMax = 1f;
        public const float BubblePopVerticalVectorMin = 0.1f;
        public const float BubblePopVerticalVectorMax = 1f;

        public const float BubblePopForce = 100f;
        
        #endregion
        
        #region AUDIO
        
        public const int IslandPopSoundTreshold = 6;
        
        #endregion
    }
}