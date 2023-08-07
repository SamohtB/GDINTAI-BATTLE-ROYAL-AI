using System.Collections.Generic;
using UnityEngine;

public enum Tank_States : int
{
    IDLE = -1,
    DEFEND_BASE,
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


    private void Start()
    {
    }

    private float CalculateDistance(Vector2Int pos1, Vector2Int pos2)
    {
        return Mathf.Abs(Vector2Int.Distance(pos1, pos2));
    }

    private void CalculateDistances(List<GameObject> bases, Vector2Int targetPos, List<float> distances)
    {
        foreach (GameObject baseObj in bases)
        {
            GridObject gridObject = baseObj.GetComponent<GridObject>();
            float dist = CalculateDistance(targetPos, gridObject.PositionOnGrid);
            distances.Add(dist);
        }

        distances.Sort();
    }

    public Tank_States CheckState()
    {
        Vector2Int agentPos = agent.PositionOnGrid;
        Vector2Int playerPos = player.PositionOnGrid;

        List<float> distanceAgentToAgentBase = new List<float>();
        List<float> distancePlayerToAgentBase = new List<float>();
        List<float> distancePlayerToPlayerBase = new List<float>();
        List<float> distanceAgentToPlayerBase = new List<float>();

        CalculateDistances(agentBases, agentPos, distanceAgentToAgentBase);
        CalculateDistances(agentBases, playerPos, distancePlayerToAgentBase);
        CalculateDistances(playerBases, playerPos, distancePlayerToPlayerBase);
        CalculateDistances(playerBases, agentPos, distanceAgentToPlayerBase);

        if (distanceAgentToPlayerBase[0] < distancePlayerToPlayerBase[0])
        {
            return Tank_States.ATTACK_BASE;
        }
        else if (distanceAgentToPlayerBase[0] < distancePlayerToPlayerBase[1])
        {
            return Tank_States.HUNT_PLAYER;
        }

        return Tank_States.IDLE;
    }
}
