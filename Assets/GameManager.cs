using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerBases;
    [SerializeField] private List<GameObject> enemyBases;

    [SerializeField] private float RespawnTimer = 5f;

    [SerializeField] private TextMeshProUGUI playerEliminations;
    [SerializeField] private TextMeshProUGUI enemyEliminations;

    [SerializeField] private TextMeshProUGUI playerBasesCount;
    [SerializeField] private TextMeshProUGUI enemyBasesCount;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private int playerSpawnPoint;
    [SerializeField] [ReadOnly] private int enemySpawnPoint;

    [SerializeField] [ReadOnly] private int playerEliminationsCount = 0;
    [SerializeField] [ReadOnly] private int enemyEliminationsCount = 0;

    [SerializeField] [ReadOnly] private int playerAliveBases = 3;
    [SerializeField] [ReadOnly] private int enemyAliveBases = 3;
 
    
    
    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_ELIMINATION, this.IncrementPoint);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_BASE_DESTROYED, this.BaseDestroyed);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_END, this.GameEnd);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_ELIMINATION);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_BASE_DESTROYED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_GAME_END);
    }

    private void Start()
    {
        RandomizeSpawn();
    }

    private void RandomizeSpawn()
    {
        playerSpawnPoint = Random.Range(0, 3);
        enemySpawnPoint = Random.Range(0, 3);

        playerBases[playerSpawnPoint].GetComponent<PlayerStart>().CreateTank(); //Spawn Initial Player Tank
        enemyBases[enemySpawnPoint].GetComponent<PlayerStart>().CreateTank();   //Spawn Initial Enemy Tank
    }

    public void GameEnd()
    {
        Debug.Log("Game End");
    }

    public void IncrementPoint(Parameters param)
    {
        //0 = High Elf Point, 1 = Dark Elf Point
        int faction_point = param.GetIntExtra(EventNames.ON_ELIMINATION, -1);

        switch(faction_point)
        {
            case 0:
                playerEliminationsCount++;
                playerEliminations.text = playerEliminationsCount.ToString();
                StartCoroutine(EnemyRespawn());
                break;

            case 1:
                enemyEliminationsCount++;
                enemyEliminations.text = enemyEliminationsCount.ToString();
                StartCoroutine(PlayerRespawn());
                break;

            default:
                Debug.LogError("Eliminations Broadcast Error");
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

    public void BaseDestroyed(Parameters param) 
    {
        int destroyed_base = param.GetIntExtra(EventNames.ON_BASE_DESTROYED, -1);

        switch(destroyed_base)
        {
            case 0:
                playerAliveBases--;
                playerBasesCount.text = playerAliveBases.ToString();
                CheckGameStatus();
                break;

            case 1:
                enemyAliveBases--;
                enemyBasesCount.text = enemyAliveBases.ToString();
                CheckGameStatus();
                break;

            default:
                Debug.LogError("Base Destroyed Broadcast Error");
                break;
        }
    }

    private void CheckGameStatus()
    {
        if(playerAliveBases <= 0 || enemyAliveBases <= 0)
        { 
            GameEnd();
        }
    }
}
