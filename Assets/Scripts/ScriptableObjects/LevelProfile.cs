// Onur Ereren - June 2024
// Popcore case

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Profile", menuName = "PopsBubble/New Level")]
public class LevelProfile : ScriptableObject
{
    [Header("Grid Starting Parameters")] public int NumberOfRows;
    public int MinimumStartingValue;
    public int MaximumStartingValue;
    [Header("Shooter Parameters")]
    public int MaximumShootValue;
}
