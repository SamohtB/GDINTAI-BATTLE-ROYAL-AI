using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Enum;

public class FactionColor : MonoBehaviour
{
    [SerializeField] private ObjectType objectType;
    [SerializeField] private List<Material> factionColor;
    [SerializeField] [ReadOnly] private Faction factionOwner;
    

    // Start is called before the first frame update
    void Start()
    {
        SetFaction();
    }

    private void SetFaction()
    {
        switch(objectType)
        {
            case ObjectType.Base:
                factionOwner = GetComponent<PlayerStart>().Faction;
                break;

            case ObjectType.Tank:
                factionOwner = GetComponent<Tank>().Faction;
                break;

            case ObjectType.Bullet:
                break;

            default:
                Debug.LogError("FACTION-BODY MISMATCH");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        SetFaction();
    }
}
