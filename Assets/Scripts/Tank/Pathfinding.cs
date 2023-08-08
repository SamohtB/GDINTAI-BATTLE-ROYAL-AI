using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathNode
{
    public int pos_x {  get; set; }
    public int pos_y { get; set; }

    public float gValue { get; set; }
    public float hValue { get; set; }
    public PathNode parentNode { get; set; }

    public float fValue
    {
        get { return gValue + hValue; }
    }

    public PathNode(int xPos, int yPos)
    {
        pos_x = xPos;
        pos_y = yPos;
    }

    public void Clear()
    {
        gValue = 0f;
        hValue = 0f;
        parentNode = null;
    }
}

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Grid targetGrid;
    [SerializeField] private GridObject enemy;

    PathNode[,] pathNodes;
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (targetGrid == null) { targetGrid = FindFirstObjectByType<Grid>(); }

        Vector2Int bounds = targetGrid.GetBounds();
        pathNodes = new PathNode[bounds.x, bounds.y];

        for (int x = 0; x < bounds.x; x++)
        {
            for (int y = 0; y < bounds.y; y++)
            {
                pathNodes[x, y] = new PathNode(x,y);
            }
        }
    }

    public List<PathNode> PathFind(Vector2Int start, Vector2Int goal)
    {
        PathNode startNode = pathNodes[start.x, start.y];
        PathNode endNode = pathNodes[goal.x, goal.y];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);
        startNode.gValue = 0;

        while(openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            for (int i = 0; i < openList.Count; i++)
            {
                if (currentNode.fValue > openList[i].fValue)
                {
                    currentNode = openList[i];
                }

                if (currentNode.fValue == openList[i].fValue && currentNode.hValue > openList[i].hValue)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == endNode)
            {
                return ReconstructPath(startNode, endNode);
            }

            List<PathNode> neighborNodes = new List<PathNode>();

            foreach(Vector2Int dir in directions)
            {
                int neighbor_X = currentNode.pos_x + dir.x;
                int neighbor_Y = currentNode.pos_y + dir.y;

                if (neighbor_X < 0 || neighbor_X >= targetGrid.GetBounds().x ||
                    neighbor_Y < 0 || neighbor_Y >= targetGrid.GetBounds().y ) { continue; }
                if (targetGrid.CheckBoundry(neighbor_X, neighbor_Y) == false) { continue; }

                PathNode neighborNode = pathNodes[neighbor_X, neighbor_Y];
                neighborNodes.Add(neighborNode);
            }

            foreach (PathNode neighbor in neighborNodes)
            {
                if(closedList.Contains(neighbor)) { continue; }
                if(!targetGrid.CheckWalkable(neighbor.pos_x, neighbor.pos_y)) { continue; }

                float tileCost = currentNode.gValue + CalculateHCost(currentNode);

                if (!openList.Contains(neighbor) || tileCost < neighbor.gValue)
                {
                    neighbor.gValue = currentNode.gValue + 1;
                    neighbor.hValue = CalculateHCost(neighbor);
                    neighbor.parentNode = currentNode;

                    if(!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    private int CalculateHCost(PathNode targetNode)
    {
        GridObject pObject = targetGrid.GetGridObject(targetNode.pos_x, targetNode.pos_y);
        if(pObject == null) { return 0; }

        switch(pObject.ObjectType)
        {
           case GridObjectType.SPEEDUP:
                return -10;
           
           case GridObjectType.HAZARD:
                return 1000;

           default:
                return 0;
        }
    }


    private List<PathNode> ReconstructPath(PathNode startNode, PathNode goalNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        return path;
    }
}


