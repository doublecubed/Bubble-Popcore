// Onur Ereren - July 2024
// Popcore case

// Script for testing HexGrid operations. Still useful to this day.

using UnityEngine;
using PopsBubble;
using Cysharp.Threading.Tasks;

public class DropdownTest : MonoBehaviour
{
    #region REFERENCES
    
    public HexGrid grid;

    #endregion
    
    #region MONOBEHAVIOUR
    
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            await MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            await MoveDown();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await Scramble();
        }
    }

    #endregion
    
    #region METHODS
    
    private async UniTask MoveDown()
    {
        await grid.MoveGridDown();        
    }

    private async UniTask MoveUp()
    {
        await grid.MoveGridUp();
    }

    private async UniTask Scramble()
    {
        await grid.ScrambleGrid();
    }
    
    #endregion
}
