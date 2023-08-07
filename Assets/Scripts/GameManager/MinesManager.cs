using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class MinesManager : MonoBehaviour
{
    [SerializeField] private Grid targetGrid;
    [SerializeField] private int mineSpawnCount = 3;
    Vector2Int gridSize;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.MINES_MINES, SpawnMines);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserver(EventNames.MINES_MINES);
    }

    private void Start()
    {
        gridSize = targetGrid.GetBounds();
    }

    public void SpawnMines()
    {
        for(int i = 0; i < mineSpawnCount; i++)
        {
            SpawnMine();
        }
    }

    public void SpawnMine()
    {
        bool spawnLocFound = false;
        int gridLocX = 0;
        int gridLocY = 0;

        while(!spawnLocFound)
        {
            gridLocX = Random.Range(0, gridSize.x);
            gridLocY = Random.Range(0, gridSize.y);
            if(!targetGrid.CheckOccupied(gridLocX, gridLocY) && targetGrid.CheckWalkable(gridLocX, gridLocY))
            {
                spawnLocFound = true;
            }
        }

        Vector3 worldCoordinates = targetGrid.GetWorldPosition(gridLocX, gridLocY);
        GameObject mine = MinesPool.SharedInstance.GetPooledObject();

        if (mine != null)
        {
            mine.transform.position = worldCoordinates;
            mine.GetComponent<GridObject>().PlaceInGrid(worldCoordinates);
            mine.SetActive(true);
        }

        Debug.Log("Mine Spawned at " + worldCoordinates);
    }
}
