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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await MoveDown();
        }
    }

    private async UniTask MoveDown()
    {
        await grid.MoveGridDown();        
    }
}
