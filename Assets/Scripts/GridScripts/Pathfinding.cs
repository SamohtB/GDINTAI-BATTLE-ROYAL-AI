using System;
using System.Collections;
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

[RequireComponent(typeof(Grid))]
public class Pathfinding : MonoBehaviour
{
    Grid gridMap;
    PathNode[,] pathNodes;
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (gridMap == null) { gridMap = GetComponent<Grid>(); }

        pathNodes = new PathNode[gridMap._width, gridMap._height];

        for (int x = 0; x < gridMap._width; x++)
        {
            for (int y = 0; y < gridMap._height; y++)
            {
                pathNodes[x, y] = new PathNode(x,y);
            }
        }
    }
    public void CalculateWalkableNodes(int startX, int startY, float range, ref List<PathNode> toHighlight)
    {
        PathNode startNode = pathNodes[startX, startY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<PathNode> neighborNodes = new List<PathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) { continue; }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false) { continue; }

                    neighborNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
            }

            for (int i = 0; i < neighborNodes.Count; i++)
            {
                if (closedList.Contains(neighborNodes[i])) { continue; }
                if (gridMap.CheckWalkable(neighborNodes[i].pos_x, neighborNodes[i].pos_y) == false) { continue; }

                float moveCost = currentNode.gValue + CalculateDistance(currentNode, neighborNodes[i]);

                if (moveCost > range) { continue; }

                if (openList.Contains(neighborNodes[i]) == false || moveCost < neighborNodes[i].gValue)
                {
                    neighborNodes[i].gValue = moveCost;
                    neighborNodes[i].parentNode = currentNode;

                    if (openList.Contains(neighborNodes[i]) == false)
                    {
                        openList.Add(neighborNodes[i]);
                    }
                }
            }
        }

        if (toHighlight != null)
        {
            toHighlight.AddRange(closedList);
        }
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = pathNodes[startX, startY];
        PathNode endNode = pathNodes[endX, endY];

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
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

            if (currentNode == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            List<PathNode> neighborNodes = new List<PathNode>();
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0) { continue; }
                    if (gridMap.CheckBoundry(currentNode.pos_x + x, currentNode.pos_y + y) == false) { continue; }

                    neighborNodes.Add(pathNodes[currentNode.pos_x + x, currentNode.pos_y + y]);
                }
            }

            for (int i = 0; i < neighborNodes.Count; i++)
            {
                if (closedList.Contains(neighborNodes[i])) { continue; }
                if (gridMap.CheckWalkable(neighborNodes[i].pos_x, neighborNodes[i].pos_y) == false) { continue; }

                float moveCost = currentNode.gValue + CalculateDistance(currentNode, neighborNodes[i]);

                if (openList.Contains(neighborNodes[i]) == false || moveCost < neighborNodes[i].gValue)
                {
                    neighborNodes[i].gValue = moveCost;
                    neighborNodes[i].hValue = CalculateDistance(neighborNodes[i], endNode);
                    neighborNodes[i].parentNode = currentNode;

                    if (openList.Contains(neighborNodes[i]) == false)
                    {
                        openList.Add(neighborNodes[i]);
                    }
                }
            }

        }

        return null;
    }
    private int CalculateDistance(PathNode currentNode, PathNode target)
    {
        int distance_x = Mathf.Abs(currentNode.pos_x - target.pos_x);
        int distance_y = Mathf.Abs(currentNode.pos_y - target.pos_y);

        if (distance_x > distance_y) { return 14 * distance_y + 10 * (distance_x - distance_y); }
        return 14 * distance_x + 10 * (distance_y - distance_x);
    }

    private List<PathNode> RetracePath(PathNode startNode, PathNode endNode) 
    {
        List<PathNode> path = new List<PathNode> ();

        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();

        return path;
    }
    public List<PathNode> TraceBackPath(int x, int y)
    {
        if (gridMap.CheckBoundry(x, y) == false) { return null; }
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = pathNodes[x, y];
        while (currentNode.parentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        
        return path;
    }

    public void Clear()
    {
        for (int x = 0; x < gridMap._width; x++)
        {
            for (int y = 0; y < gridMap._height; y++)
            {
                pathNodes[x, y].Clear();
            }
        }
    }
}
