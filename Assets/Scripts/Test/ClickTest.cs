using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PopsBubble;

public class ClickTest : MonoBehaviour
{
    private HexGrid grid;

    private List<HexCell> cells;
    
    private void Start()
    {
        grid = FindObjectOfType<HexGrid>();
        HexCell thisCell = grid.HexCell(transform.position);
        thisCell.objectTransform = transform;
    }

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        
        HexCell centralCell = grid.HexCell(transform.position);

        cells = grid.Neighbours(centralCell);

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].objectTransform.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

    }

    private void OnMouseUp()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.08f);
        
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].objectTransform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.08f);
        }
    }
}
