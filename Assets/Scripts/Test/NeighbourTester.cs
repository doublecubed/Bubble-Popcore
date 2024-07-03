using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopsBubble;

public class NeighbourTester : MonoBehaviour
{
    public HexGrid grid;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            HexCell targetCell = grid.CellFromCoordinates(grid.HexCoordinate(pos));

            targetCell.Bubble.GetComponent<SpriteRenderer>().color = Color.green;

            HexCell[] neighbours = grid.NeighbourCells(targetCell);

            Debug.Log($"Cell {targetCell.Coordinates}");
            
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (neighbours[i] == null)
                {
                    Debug.Log($"neighbour {i} is absent");
                    continue;
                }

                if (neighbours[i].Bubble == null)
                {
                    Debug.Log($"neighbour {i} has no bubble");
                    continue;
                }

                Debug.Log($"neighbour {i} is {neighbours[i].Coordinates}");
                neighbours[i].Bubble.GetComponent<SpriteRenderer>().color = Color.yellow;

            }
            
        }
    }
}
