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

    public void PlaceInGrid(Vector3 postion)
    {
        PositionOnGrid = TargetGrid.GetGridPosition(postion);
        TargetGrid.PlaceObject(TargetGrid.GetGridPosition(postion), this);
    }

    public void RemoveFromGrid(Vector3 position)
    {
        TargetGrid.RemoveObject(TargetGrid.GetGridPosition(position), this);
    }
}
