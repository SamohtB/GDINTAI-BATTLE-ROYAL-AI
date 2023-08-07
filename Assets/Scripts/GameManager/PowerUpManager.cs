using UnityEngine;
using static Enum;

public class PowerUpManager : MonoBehaviour
{
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

        GameObject powerUp = PowerUpPool.SharedInstance.GetPooledObject((PowerUpType)powerUpType);
        if (powerUp != null)
        {
            powerUp.transform.position = worldCoordinates;
            powerUp.GetComponent<GridObject>().PlaceInGrid(worldCoordinates);
            powerUp.SetActive(true);
        }

        Debug.Log("Spawned at " + worldCoordinates + " as " + (PowerUpType)powerUpType);

        powerUpSpawnTime = Random.Range(15, 20);
        Debug.Log("NEW SPAWN TIME: " + powerUpSpawnTime);
    }
}
