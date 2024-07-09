// Onur Ereren - June 2024
// Popcore case

using UnityEngine;

[CreateAssetMenu(fileName = "New Level Profile", menuName = "PopsBubble/New Level")]
public class LevelProfile : ScriptableObject
{
    [Header("Grid Cell Value Starting Parameters")] public int NumberOfRows;
    public int MinimumStartingValue;
    public int MaximumStartingValue;
    
    [Header("Shooter Value Parameters")]
    public int MaximumShootValue;

    [Header("Dropdown Frequency")] 
    public int DropdownFrequency;
}
