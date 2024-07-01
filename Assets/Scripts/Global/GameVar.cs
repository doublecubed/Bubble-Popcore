// Onur Ereren - June 2024
// Popcore case

// This script provides the global values throughout the game.

using UnityEngine;

namespace PopsBubble
{
    public static class GameVar
    {
        private const int BaseValue = 2;

        public static int Value(int power)
        {
            return (int)Mathf.Pow(BaseValue, power);
        }
    }
}