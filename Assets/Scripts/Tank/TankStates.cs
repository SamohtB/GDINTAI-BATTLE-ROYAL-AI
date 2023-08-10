using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using static Enum;


public enum Tank_States : int
{
    IDLE = -1,
    ATTACK_BASE,
    HUNT_PLAYER,
    SEEK_POWER_UP,
    FIRE
}

public class TankStates : MonoBehaviour
{
    [SerializeField] private GridObject agent;
    [SerializeField] private GridObject player;
    [SerializeField] private List<GridObject> playerBases;
    [SerializeField] private List<GridObject> agentBases;
    [SerializeField] private List<GridObject> pointsOfInterests;
    [SerializeField] private Tank_States state;

    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private Grid targetGrid;
    [SerializeField] private PlayerDetect playerDetect;

    private float distanceBetweenAgentBases;
    private float distanceBetweenPlayerBases;

    public Tank_States GetState() { return state; }

    private void Start()
    {
        RecalculateBaseDistances();
    }

    public Tank_States GetCurrentState()
    {
        float playerMinDistanceToPlayerBases = GetMinDistanceToObjective(Faction.HighElf, Faction.HighElf);
        float agentMinDistanceToAgentBases = GetMinDistanceToObjective(Faction.DarkElf, Faction.HighElf);

        if(playerDetect.PlayerIsDetected && player.GetComponent<Tank>().IsAlive)
        {
            Debug.Log("Player Detected");
            state = Tank_States.FIRE; return state;
        }
        else if(!player.GetComponent<Tank>().IsAlive || agentMinDistanceToAgentBases < playerMinDistanceToPlayerBases)
        {
            Debug.Log("Attacking Base");
            state = Tank_States.ATTACK_BASE;
            return state;
        }
        else if(agentMinDistanceToAgentBases > playerMinDistanceToPlayerBases)
        {
            if(pointsOfInterests.Count > 0)
            {
                state = Tank_States.SEEK_POWER_UP;
            }
            Debug.Log("Looking for Player");
            return Tank_States.HUNT_PLAYER;
        }

        return Tank_States.IDLE;
    }

    private float GetMinDistanceToObjective(Faction owner, Faction bases)
    {
        GridObject sourceEntity = (owner == Faction.DarkElf) ? agent : player;
        List<GridObject> targetList = (bases == Faction.DarkElf) ? agentBases : playerBases;
        float distanceToAdd = (bases == Faction.DarkElf) ? distanceBetweenAgentBases : distanceBetweenPlayerBases;

        if(sourceEntity.gameObject.activeSelf == false)
            return 1000;

        return distanceToAdd + GetClosestBaseDistance(sourceEntity, targetList);
    }


    private void RecalculateBaseDistances()
    {
        distanceBetweenAgentBases = CalculateDistanceBetweenActiveBases(agentBases);
        distanceBetweenPlayerBases = CalculateDistanceBetweenActiveBases(playerBases);
    }

    private float CalculateDistanceBetweenActiveBases(List<GridObject> bases)
    {
        bases = bases.OrderBy(baseObj => baseObj.GetComponent<Renderer>().enabled).ToList();
        float distanceBetweenBases = 0;
        bool inactiveFound = false;

        for (int i = 0; i < bases.Count - 1 && !inactiveFound; i++)
        {
            if (!bases[i].GetComponent<Renderer>().enabled)
            {
                inactiveFound = true;
            }

            if (bases[i + 1].GetComponent<Renderer>().enabled)
            {
                distanceBetweenBases += pathfinding.PathFind(bases[i].PositionOnGrid, bases[i + 1].PositionOnGrid).Count;
            }
        }

        return distanceBetweenBases;
    }

    private float GetClosestBaseDistance(GridObject origin, List<GridObject> bases)
    {
        float closestDistance = float.MaxValue;

        foreach (GridObject baseObject in bases)
        {
            if (baseObject.GetComponent<Renderer>().enabled)
            {
                float distance = pathfinding.PathFind(origin.PositionOnGrid, baseObject.PositionOnGrid).Count;
                closestDistance = Mathf.Min(closestDistance, distance);
            }
        }

        return closestDistance;
    }    

}
