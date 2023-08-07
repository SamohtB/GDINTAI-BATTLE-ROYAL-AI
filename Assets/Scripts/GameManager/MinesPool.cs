using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class MinesPool : MonoBehaviour
{
    public static MinesPool SharedInstance;
    [SerializeField] private GameObject minePrefab;
    [SerializeField] private int defaultSize = 20;
    [SerializeField] private Grid targetGrid;
    [SerializeField] private GameObject container;
    private List<GameObject> minePool;
    
    void Awake()
    {
        SharedInstance = this;
        minePool = new List<GameObject>();
    }

    private void Start()
    {
        for (int j = 0; j < defaultSize; j++)
        {
            GameObject tmp = Instantiate(minePrefab, container.transform);
            tmp.SetActive(false);
            tmp.GetComponent<GridObject>().TargetGrid = targetGrid;
            minePool.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < defaultSize; i++)
        {
            if (minePool[i].activeInHierarchy)
            {
                return minePool[i];
            }
        }
        return null;
    }
}
