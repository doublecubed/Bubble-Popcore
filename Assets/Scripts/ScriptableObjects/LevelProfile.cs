using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Profile", menuName = "PopsBubble/New Level")]
public class LevelProfile : ScriptableObject
{
    public int NumberOfRows;
    public int MinimumStartingValue;
    public int MaximumShootValue;
}
