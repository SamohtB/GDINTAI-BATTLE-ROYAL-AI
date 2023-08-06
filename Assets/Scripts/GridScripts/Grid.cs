using System.Collections.Generic;
using UnityEngine;

/*
 * Grid System - 2D Array with nodes in each element
 */
public class Grid : MonoBehaviour
{
    [Header("Grid Properties")]
    [Tooltip("Level Size")]
    [SerializeField] private int _width = 10;
    [Tooltip("Level Size")]
    [SerializeField] private int _height = 10;
    [Tooltip("Size of each tile")]
    [SerializeField] private float _cellSize;
    [Tooltip("Layer mask for walls or obstacles. Objects set here will be unpassable")]
    [SerializeField] private LayerMask _obstacleLayer;
    [Tooltip("Layer mask for floor")]
    [SerializeField] private LayerMask _terrainLayer;
    private Node[,] grid;
    [SerializeField] private bool gizmoSwitch = false;
    [SerializeField] private float cellMultiplier;

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Node[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                grid[x, y] = new Node();
                grid[x, y].Passable = true;
                grid[x, y].GridObject = null;
            }
        }
 
        CheckPassableTerrain();
    }

    /*
     * checks each tile if there are any obstacles in its area
     * obstacles are counted even if they don't cover the whole tile
     */
    private void CheckPassableTerrain()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 worldPosition = GetWorldPosition(x, y);
                bool passable = !Physics.CheckBox(worldPosition, Vector3.one / 2 * _cellSize * 0.9f, Quaternion.identity, _obstacleLayer);
                grid[x, y].Passable = passable;
            }
        }
    }

    /* Sends rays downward, first collision with terrain is the elevation of the tile 
    private void CalculateElevation()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Ray ray = new Ray(GetWorldPosition(x, y) + Vector3.up * 100f, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, float.MaxValue, _terrainLayer))
                {
                    grid[x, y].elevation = hit.point.y;
                }
            }
        }
    }
    */
    private void OnDrawGizmos()
    {
        if (grid == null && gizmoSwitch)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.DrawCube(pos, Vector3.one * cellMultiplier);
                }
            }
        }
        else if (grid != null)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 pos = GetWorldPosition(x, y, true);
                    Gizmos.color = grid[x, y].Passable ? Color.white : Color.red;
                    Gizmos.DrawCube(pos, Vector3.one * cellMultiplier);
                }
            }
        }

        
    }
    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        return new Vector3(x * _cellSize, 0f, y * _cellSize);
    }

    public bool CheckWalkable(int pos_x, int pos_y)
    {
        return grid[pos_x, pos_y].Passable;
    }

    public bool CheckOccupied(int pos_x, int pos_y)
    {
        if(grid[pos_x, pos_y].GridObject == null)
        { 
           return false; 
        }

        return true;
    }

    public void PlaceObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        if (CheckBoundry(positionOnGrid))
        {
            grid[positionOnGrid.x, positionOnGrid.y].GridObject = gridObject;
            grid[positionOnGrid.x, positionOnGrid.y].Passable = false;
            Debug.Log("Added " + gridObject.name + " at " + positionOnGrid);

        }
        else
        {
            Debug.Log("OUT OF BOUNDS ACCESS");
        }
    }
    public void RemoveObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        if (CheckBoundry(positionOnGrid))
        {
            grid[positionOnGrid.x, positionOnGrid.y].GridObject = null;
            grid[positionOnGrid.x, positionOnGrid.y].Passable = true;
        }
        else 
        {
            Debug.Log("OUT OF BOUNDS ACCESS");
        }
    }

    public bool CheckBoundry(Vector2Int positionOnGrid)
    {
        if (positionOnGrid.x < 0 || positionOnGrid.x >= _width)
        { 
            return false; 
        }
        if (positionOnGrid.y < 0 || positionOnGrid.y >= _height)
        {
            return false;
        }

        return true;
    }

    public bool CheckBoundry(int xPos, int yPos)
    {
        if (xPos < 0 || xPos >= _width)
        {
            return false;
        }
        if (yPos < 0 || yPos >= _height)
        {
            return false;
        }

        return true;
    }

    public Vector2Int GetBounds()
    {
        return new Vector2Int(_width, _height);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += _cellSize / 2;
        worldPosition.z += _cellSize / 2;
        Vector2Int positionOnGrid = new Vector2Int((int)(worldPosition.x / _cellSize), (int)(worldPosition.z / _cellSize));
        return positionOnGrid;
    }

    public GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        if (CheckBoundry(gridPosition))
        {
            GridObject gridObject = grid[gridPosition.x, gridPosition.y].GridObject;
            return gridObject;
        }

        return null;
    }

    //public List<Vector3> ConvertPathNodeToTargetPositions(List<PathNode> path)
    //{
      //  List<Vector3> worldPositions = new List<Vector3>();

     //   for (int i = 0; i < path.Count; i++)
     //   {
     //       worldPositions.Add(GetWorldPosition(path[i].pos_x, path[i].pos_y, true));
      //  }

     //   return worldPositions;
        
  //  }
   
}