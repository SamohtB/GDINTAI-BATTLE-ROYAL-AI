using System.Collections.Generic;
using UnityEngine;

public enum Tank_States : int
{
    IDLE = -1,
    ATTACK_BASE,
    HUNT_PLAYER,
    SEEK_POWER_UP,
}

public class TankStates : MonoBehaviour
{
    [SerializeField] private GridObject agent;
    [SerializeField] private GridObject player;
    [SerializeField] private List<GameObject> playerBases;
    [SerializeField] private List<GameObject> agentBases;
    [SerializeField] private List<GridObject> pointsOfInterests;

    private Pathfinding pathfinding;

}
