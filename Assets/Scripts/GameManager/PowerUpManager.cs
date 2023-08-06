using System.Collections;
using UnityEngine;
using static Enum;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private Grid targetGrid;

    private float powerUpSpawnTime;
    private float ticks;

    Vector2Int gridSize;

    private void Start()
    {
        gridSize = targetGrid.GetBounds();
        powerUpSpawnTime = 15.0f;
    }

    private void Update()
    {
        ticks += Time.deltaTime;
        if(ticks >= powerUpSpawnTime) 
        {
            ticks = 0.0f;
            SpawnPowerUp();
        }
    }

    private void SpawnPowerUp()
    {
        bool spawnLocFound = false;
        int gridLocX = 0;
        int gridLocY = 0;

        while(!spawnLocFound)
        {
            gridLocX = Random.Range(0, gridSize.x);
            gridLocY = Random.Range(2, gridSize.y - 2);
            if(!targetGrid.CheckOccupied(gridLocX, gridLocY) && targetGrid.CheckWalkable(gridLocX, gridLocY))
            {
                spawnLocFound = true;
            }
        }
        
        int powerUpType = Random.Range(0, 5);

        Vector3 worldCoordinates = targetGrid.GetWorldPosition(gridLocX, gridLocY);
        GameObject powerUp = Instantiate(powerUpPrefab);
        powerUp.transform.position = worldCoordinates;
        powerUp.GetComponent<PowerUp>().PlaceInGrid();
       
        Debug.Log("Spawned at " + worldCoordinates + " as " + (PowerUpType)powerUpType);

        powerUpSpawnTime = Random.Range(15, 20);
        Debug.Log("NEW SPAWN TIME: " + powerUpSpawnTime);
    }
}
