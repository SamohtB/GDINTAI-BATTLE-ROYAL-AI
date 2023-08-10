using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI playerEliminations;
    [SerializeField] private TextMeshProUGUI enemyEliminations;

    [SerializeField] private TextMeshProUGUI playerBasesCount;
    [SerializeField] private TextMeshProUGUI enemyBasesCount;

    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endText;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private int playerEliminationsCount = 0;
    [SerializeField] [ReadOnly] private int enemyEliminationsCount = 0;

    [SerializeField] [ReadOnly] private int playerAliveBases = 3;
    [SerializeField] [ReadOnly] private int enemyAliveBases = 3;

    private SpawnManager spawnManager;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_ELIMINATION, this.IncrementPoint);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_BASE_DESTROYED, this.BaseDestroyed);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_END, this.GameEnd);

        spawnManager = GetComponent<SpawnManager>();
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_ELIMINATION);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_BASE_DESTROYED);
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_GAME_END);
    }

    void Start()
    {
        playerBasesCount.text = playerAliveBases.ToString();
        enemyBasesCount.text = enemyAliveBases.ToString();
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
                spawnManager.RespawnCharacter(Enum.Faction.DarkElf);
                break;

            case 1:
                enemyEliminationsCount++;
                enemyEliminations.text = enemyEliminationsCount.ToString();
                spawnManager.RespawnCharacter(Enum.Faction.HighElf);
                break;

            case 2:
                spawnManager.RespawnCharacter(Enum.Faction.HighElf);
                break;

            case 3:
                spawnManager.RespawnCharacter(Enum.Faction.DarkElf);
                break;


            default:
                Debug.LogError("Eliminations Broadcast Error");
                break;
        }
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

    public void GameEnd()
    {
        Debug.Log("Game End");
        Time.timeScale = 0.0f;
        endPanel.SetActive(true);
        
        if(enemyAliveBases <= 0)
        {
            endText.text = "YOU WIN";
        }
        else if(playerAliveBases <= 0)
        {
            endText.text = "YOU LOSE";
        }
        else if(playerEliminationsCount < enemyEliminationsCount)
        {
            endText.text = "YOU LOSE";
        }
        else if(playerEliminationsCount > enemyEliminationsCount)
        {
            endText.text = "YOU WIN";
        }
        else
        {
            endText.text = "TIE";
        }
    }
}
