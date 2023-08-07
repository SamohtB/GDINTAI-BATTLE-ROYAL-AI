using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerBases;
    [SerializeField] private List<GameObject> enemyBases;

    [SerializeField] private float RespawnTimer = 5f;
    [SerializeField] private Grid targetGrid;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private int playerSpawnPoint;
    [SerializeField] [ReadOnly] private int enemySpawnPoint;

    private Vector2Int gridSize;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.BRING_CHAOS, RandomizeBaseLocations);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.BRING_CHAOS);
    }

    private void Start()
    {
        RandomizeSpawn();
        gridSize = targetGrid.GetBounds();
    }

    private void RandomizeSpawn()
    {
        playerSpawnPoint = Random.Range(0, 3);
        enemySpawnPoint = Random.Range(0, 3);

        playerBases[playerSpawnPoint].GetComponent<PlayerStart>().CreateTank(); //Spawn Initial Player Tank
        enemyBases[enemySpawnPoint].GetComponent<PlayerStart>().CreateTank();   //Spawn Initial Enemy Tank
    }

    public void RespawnCharacter(Faction faction)
    {
        switch (faction) 
        {
            case Faction.HighElf:
                StartCoroutine(PlayerRespawn());
                break;

            case Faction.DarkElf:
               StartCoroutine(EnemyRespawn());
                break;
        }
    }

    IEnumerator PlayerRespawn()
    {
        yield return new WaitForSeconds(RespawnTimer);
        playerBases[playerSpawnPoint].GetComponent<PlayerStart>().CreateTank();
    }

    IEnumerator EnemyRespawn()
    {
        yield return new WaitForSeconds(RespawnTimer);
        enemyBases[enemySpawnPoint].GetComponent<PlayerStart>().CreateTank();
    }

    private void RandomizeBaseLocations()
    {
        int gridLocX;
        int gridLocY;

        if(targetGrid == null)
        {
            targetGrid = FindObjectOfType<Grid>();
        }
         
        foreach(GameObject pBase in playerBases)
        {
            bool spawnLocFound = false;

            while(!spawnLocFound)
            {
                gridLocX = Random.Range(0, gridSize.x);
                gridLocY = Random.Range(2, gridSize.y - 2);

                if(!targetGrid.CheckOccupied(gridLocX, gridLocY) && targetGrid.CheckWalkable(gridLocX, gridLocY))
                {
                    spawnLocFound = true;
                }

                pBase.GetComponent<GridObject>().RemoveFromGrid(pBase.transform.position);
                pBase.transform.position = targetGrid.GetWorldPosition(gridLocX, gridLocY);
                pBase.GetComponent<GridObject>().PlaceInGrid(pBase.transform.position);

                Debug.Log(pBase.name + " moved to " + pBase.transform.position);
            }
        }

        foreach(GameObject pBase in enemyBases)
        {
            bool spawnLocFound = false;

            while(!spawnLocFound)
            {
                gridLocX = Random.Range(0, gridSize.x);
                gridLocY = Random.Range(2, gridSize.y - 2);

                if(!targetGrid.CheckOccupied(gridLocX, gridLocY) && targetGrid.CheckWalkable(gridLocX, gridLocY))
                {
                    spawnLocFound = true;
                }

                pBase.GetComponent<GridObject>().RemoveFromGrid(pBase.transform.position);
                pBase.transform.position = targetGrid.GetWorldPosition(gridLocX, gridLocY);
                pBase.GetComponent<GridObject>().PlaceInGrid(pBase.transform.position);

                Debug.Log(pBase.name + " moved to " + pBase.transform.position);
            }
        }
    }
}
