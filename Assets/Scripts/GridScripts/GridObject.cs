using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public Grid TargetGrid 
    {  
        get { return _grid; } 
        private set { _grid = value; } 
    }

    public Vector2Int PositionOnGrid {  get; set; }

    [SerializeField] private Grid _grid;

    public void PlaceInGrid()
    {
        PositionOnGrid = TargetGrid.GetGridPosition(transform.position);
        TargetGrid.PlaceObject(PositionOnGrid, this);
        Vector3 pos = TargetGrid.GetWorldPosition(PositionOnGrid.x, PositionOnGrid.y, true);
        transform.position = pos;
    }

    public void RemoveFromGrid()
    {
        TargetGrid.RemoveObject(PositionOnGrid, this);
    }
}
