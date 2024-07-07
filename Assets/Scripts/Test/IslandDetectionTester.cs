using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopsBubble;

public class IslandDetectionTester : MonoBehaviour
{
    private IslandCalculator calculator;
    private HexGrid grid;
    
    // Start is called before the first frame update
    void Start()
    {
        calculator = DependencyContainer.IslandCalculator as IslandCalculator;
        grid = DependencyContainer.Grid;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return;
            Vector2Int coordinates = grid.HexCoordinate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            HexCell cell = grid.CellMap[coordinates];

            bool result = calculator.IsAnIsland(cell);
            
            Debug.Log(result);
        }
    }
}
