using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static Enum;

/* 7-P0 Lion */
public class AITank : Tank
{
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private List<GridObject> playerBases;
    private List<PathNode> queuedPath;

    private void Start()
    {
        queuedPath = new List<PathNode>();
    }

    private void Update()
    {
        if(pathfinding == null) { pathfinding = GetComponent<Pathfinding>(); }

        if (!TankMoving())
        {
            queuedPath = pathfinding.PathFind(this.PositionOnGrid, playerBases[0].PositionOnGrid);
        }
    }

    private Direction TranslateNodeToDirection(PathNode node)
    {
        int deltaX = node.pos_x - PositionOnGrid.x;
        int deltaY = node.pos_y - PositionOnGrid.y;

        if (deltaX == 1 && deltaY == 0)
        {
            return Direction.East;
        }
        else if (deltaX == -1 && deltaY == 0)
        {
            return Direction.West;
        }
        else if (deltaX == 0 && deltaY == 1)
        {
            return Direction.North;
        }
        else if (deltaX == 0 && deltaY == -1)
        {
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
