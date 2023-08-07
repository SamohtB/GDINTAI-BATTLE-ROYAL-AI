using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int pos_x;
    public int pos_y;

    public float gValue;
    public float hValue;
    public PathNode parentNode;

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

                float tileCost = currentNode.gValue + CalculateHCost(currentNode, neighbor);

                if (!openList.Contains(neighbor) || tileCost < neighbor.gValue)
                {
                    neighbor.gValue = tileCost;
                    neighbor.hValue = CalculateHCost(neighbor, endNode);
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

    private int CalculateHCost(PathNode currentNode, PathNode targetNode)
    {
        int distance_x = Mathf.Abs(currentNode.pos_x - targetNode.pos_x);
        int distance_y = Mathf.Abs(currentNode.pos_y - targetNode.pos_y);

        if (distance_x > distance_y) { return 14 * distance_y + 10 * (distance_x - distance_y); }
        return 14 * distance_x + 10 * (distance_y - distance_x);
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


