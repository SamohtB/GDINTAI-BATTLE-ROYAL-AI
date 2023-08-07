using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static Enum;

public class PowerUpPool : MonoBehaviour
{
    public static PowerUpPool SharedInstance;
    /* 0 = SpeedUp, 1 = SpeedDown, 2 = Shield, 3 = Mines, 4 = Chaos */
    [SerializeField] private List<GameObject> powerUpPrefab;
    [SerializeField] private int defaultSize = 10;
    [SerializeField] private Grid targetGrid;
    [SerializeField] private GameObject container;

    private List<List<GameObject>> powerUpPools;
    
    void Awake()
    {
        SharedInstance = this;
        powerUpPools = new List<List<GameObject>>();
    }

    private void Start()
    {
        for(int i = 0; i < powerUpPrefab.Count; i++)
        {
            List<GameObject> pool = new List<GameObject>();
            GameObject tmp;
            for(int j = 0; j < defaultSize; j++)
            {
                tmp = Instantiate(powerUpPrefab[i], container.transform);
                tmp.SetActive(false);
                tmp.GetComponent<GridObject>().TargetGrid = targetGrid;
                pool.Add(tmp);
            }

            powerUpPools.Add(pool);
        }
        
    }

    public GameObject GetPooledObject(PowerUpType EType)
    {
        for (int i = 0; i < defaultSize; i++)
        {
            if (!powerUpPools[(int)EType][i].activeInHierarchy)
            {
                return powerUpPools[(int)EType][i];
            }
        }
        return null;
    }
}
