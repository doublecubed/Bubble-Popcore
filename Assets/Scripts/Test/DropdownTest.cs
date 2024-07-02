using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopsBubble;
using Cysharp.Threading.Tasks;

public class DropdownTest : MonoBehaviour
{
    public HexGrid grid;

    // Update is called once per frame
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
}
