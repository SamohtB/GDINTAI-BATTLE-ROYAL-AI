using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Grid targetGrid;
    [SerializeField] private GridObject agent;
    [SerializeField] private GridObject enemy;

    Node[,] grid;
    Vector2Int bounds;
    Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

    private void Start()
    {
        grid = targetGrid.GetGrid();
        bounds = targetGrid.GetBounds();
        agent = GetComponent<GridObject>();
    }

    public List<Vector2Int> PathFind(Vector2Int start, Vector2Int goal)
    {
        List<Node> result = new List<Node>();

        List<Vector2Int> openList = new List<Vector2Int>();
        List<Vector2Int> closedList = new List<Vector2Int>();

        openList.Add(start);
        grid[start.x, start.y].GCost = 0;
        grid[start.x, start.y].HCost = 0;

        while(openList.Count > 0)
        {
            Vector2Int currentNode = openList.OrderBy(node => grid[node.x, node.y].FCost)
                .ThenBy(node => grid[node.x, node.y].HCost).First();

            openList.Remove(currentNode);

            if(currentNode == goal)
            {
                return ReconstructPath(currentNode);
            }

            List<Vector2Int> neighbors = GetNeighbors(currentNode);

            foreach (Vector2Int neighbor in neighbors)
            {
                float newGCost = grid[currentNode.x, currentNode.y].GCost + 1;
                float newHCost = CalculateHCost(neighbor, goal);

                if (!closedList.Contains(neighbor) || newGCost < grid[neighbor.x, neighbor.y].GCost)
                {
                    grid[neighbor.x, neighbor.y].GCost = newGCost;
                    grid[neighbor.x, neighbor.y].HCost = newHCost;
                    grid[neighbor.x, neighbor.y].Parent = currentNode;

                    if(!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        return new List<Vector2Int>();
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighborPos = position + dir;
            if(IsValidPosition(neighborPos))
            {
                if(grid[neighborPos.x, neighborPos.y].Passable)
                {
                    neighbors.Add(neighborPos);
                }
            }
        }

        return neighbors;
    }

    private float CalculateHCost(Vector2Int tilePosition, Vector2Int goal)
    {
        if(enemy == null)
        {
            return CalculateTilePotential(tilePosition, goal) + CalculatePlayerLineOfSight(tilePosition);
        }
        else
        {
            return CalculateTilePotential(tilePosition, agent.PositionOnGrid, enemy.PositionOnGrid, goal);
        }
    }

    private float CalculateTilePotential(Vector2Int tilePosition, Vector2Int agentPosition, Vector2Int playerPosition, Vector2Int goalPosition)
    {
        float agentDistance = Vector2Int.Distance(tilePosition, agentPosition);
        float playerDistance = Vector2Int.Distance(tilePosition, playerPosition);
        float goalDistance = Vector2Int.Distance(tilePosition, goalPosition);

        float potential = agentDistance - playerDistance + goalDistance;
        return potential;
    }

    private float CalculateTilePotential(Vector2Int tilePosition, Vector2Int goalPosition)
    {
        return Vector2Int.Distance(tilePosition, goalPosition);
    }

    private float CalculatePlayerLineOfSight(Vector2Int tilePosition)
    {
        int enemyPosX = enemy.PositionOnGrid.x;
        int enemyPosY = enemy.PositionOnGrid.y;

        /* Not In Line With The Player */
        if(tilePosition.x !=  enemyPosX || tilePosition.y != enemyPosY) { return 0; }

        /* check Y Axis */
        if(tilePosition.x == enemy.PositionOnGrid.x)
        {
            if(CheckYAxis(tilePosition))
            {
                return -100;
            }
        }
        else if(tilePosition.y == enemyPosY)
        {
            if(CheckXAxis(tilePosition))
            {
                return -100;
            }
        }
            
        return 0;
    }

    private bool CheckYAxis(Vector2Int tilePosition)
    {
        int enemyPosY = enemy.PositionOnGrid.y;

        /* check up */
        if (tilePosition.y > enemyPosY)
        {
            for (int i = 1; tilePosition.y != enemyPosY; i++)
            {
                if (!grid[tilePosition.x, tilePosition.y - i].Passable)
                {
                    return true;
                }
            }
        }

        /* check down */
        if (tilePosition.y < enemyPosY)
        {
            for (int i = 1; tilePosition.y != enemyPosY; i++)
            {
                if (!grid[tilePosition.x, tilePosition.y + i].Passable)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckXAxis(Vector2Int tilePosition)
    {
        int enemyPosX = enemy.PositionOnGrid.x;

        /* check right */
        if (tilePosition.x > enemyPosX)
        {
            for (int i = 1; tilePosition.x != enemyPosX; i++)
            {
                if (!grid[tilePosition.x - i, tilePosition.y].Passable)
                {
                    return true;
                }
            }
        }

        /* check left */
        if (tilePosition.x < enemyPosX)
        {
            for (int i = 1; tilePosition.x != enemyPosX; i++)
            {
                if (!grid[tilePosition.x  + i, tilePosition.y].Passable)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Vector2Int> ReconstructPath(Vector2Int goalNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentNode = goalNode;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = grid[currentNode.x, currentNode.y].Parent;
        }

        path.Reverse();
        return path;
    }

    private bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < bounds.x && position.y >= 0 && position.y < bounds.y;
    }

    public void AssignAgent(GridObject enemy)
    {
        this.enemy = enemy;
    }
}


