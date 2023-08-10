using UnityEngine;
using UnityEngine.UIElements;


public enum GridObjectType : int
{
    NONE = -1,
    TANK = 0,
    BASE,
    SPEEDUP,
    HAZARD,
    POWERUP
}

public class GridObject : MonoBehaviour
{
    public Grid TargetGrid 
    {  
        get { return _grid; } 
        set { _grid = value; } 
    }

    public GridObjectType ObjectType
    {
        get { return _type; }
        set { _type = value; }
    }

    public Vector2Int PositionOnGrid {  get; set; }

    [SerializeField] [ReadOnly] protected Grid _grid;
    [SerializeField] private GridObjectType _type;

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
