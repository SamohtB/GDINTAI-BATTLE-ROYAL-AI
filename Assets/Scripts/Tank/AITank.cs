using System.Collections.Generic;
using UnityEngine;
using static Enum;

/* 7-P0 Lion */
public class AITank : Tank
{
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private TankStates tankStates;
    [SerializeField] private GridObject player;
    [SerializeField] private List<GridObject> playerBases;
    [SerializeField] private List<GridObject> agentBases;

    [SerializeField] private GridObject DebugFind;

    private void Update()
    {
        ticks += Time.deltaTime;
        if(IsAlive)
        {
            AI_PathFinding();    
        }
        
    }


    private void AI_PathFinding()
    {
        if (pathfinding == null) { pathfinding = GetComponent<Pathfinding>(); }

        if (!TankMoving())
        {
            PathNode nextTile = DecideDirection();

            if (nextTile != null)
            {
                MoveAgent(TranslateNodeToDirection(nextTile));

                if(GetComponent<TankStates>().GetState() == Tank_States.FIRE)
                {
                    Fire();
                }
            }
        }
    }

    private PathNode DecideDirection()
    {
        Tank_States currentState = tankStates.GetCurrentState();

        switch(currentState)
        {
            case Tank_States.FIRE:
                return pathfinding.PathFind(PositionOnGrid, player.PositionOnGrid)[0];

            case Tank_States.ATTACK_BASE:
                return GetClosestBasePathNode(this, playerBases);

            case Tank_States.HUNT_PLAYER:
                return pathfinding.PathFind(PositionOnGrid, player.PositionOnGrid)[0];

            case Tank_States.SEEK_POWER_UP:
                return null;

            default:
                return null;
        }
    }

    private PathNode GetClosestBasePathNode(GridObject origin, List<GridObject> bases)
    {
        float closestDistance = float.MaxValue;
        PathNode closestPathNode = null;

        for (int i = 0; i < bases.Count; i++)
        {
            if (bases[i].GetComponent<Renderer>().enabled)
            {
                List<PathNode> path = pathfinding.PathFind(origin.PositionOnGrid, bases[i].PositionOnGrid);
                Debug.Log("Path start!");
                if (path != null && path.Count > 0)
                {
                    float distance = path.Count;

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPathNode = path[0];
                    }
                }
                
            }
        }
        Debug.Log("Path Found");

        return closestPathNode;
    }

    private Direction TranslateNodeToDirection(PathNode node)
    {
        int deltaX = node.pos_x - PositionOnGrid.x;
        int deltaY = node.pos_y - PositionOnGrid.y;

        if (deltaX == 1 && deltaY == 0)
        {
            Debug.Log("Moving East");
            return Direction.East;
        }
        else if (deltaX == -1 && deltaY == 0)
        {
            Debug.Log("Moving West");
            return Direction.West;
        }
        else if (deltaX == 0 && deltaY == 1)
        {
            Debug.Log("Moving North");
            return Direction.North;
        }
        else if (deltaX == 0 && deltaY == -1)
        {
            Debug.Log("Moving South");
            return Direction.South;
        }


        return Direction.NONE;
    }

    private void MoveAgent(Direction moveDir)
    {
        switch (moveDir) 
        {
            case Direction.North:
                MoveUp();
                break;

            case Direction.South:
                MoveDown();
                break;
            case Direction.West:
                MoveLeft();
                break;
            case Direction.East:
                MoveRight();
                break;
            default:
                break;
        }
    }
}
