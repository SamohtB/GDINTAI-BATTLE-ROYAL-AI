using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public Grid targetGrid;
    public Vector2Int positionOnGrid;


    private void Start()
    {
        targetGrid = FindFirstObjectByType<Grid>();
    }

    public void PlaceInGrid()
    {
        if (targetGrid == null)
        {
            targetGrid = FindFirstObjectByType<Grid>();
        }
        positionOnGrid = targetGrid.GetGridPosition(transform.position);
        targetGrid.PlaceObject(positionOnGrid, this);
        Vector3 pos = targetGrid.GetWorldPosition(positionOnGrid.x, positionOnGrid.y, true);
        transform.position = pos;
    }

    public void RemoveFromGrid()
    {
        targetGrid.RemoveObject(positionOnGrid, this);
    }
}
