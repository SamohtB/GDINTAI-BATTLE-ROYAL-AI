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
        ApplyFactionColor();
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

    private void ApplyFactionColor()
    {
        switch (factionOwner) 
        {
            case Faction.HighElf:
                gameObject.GetComponent<Renderer>().material = factionColor[1];
                break;

            case Faction.DarkElf:
                gameObject.GetComponent<Renderer>().material = factionColor[2];
                break;

            default:
                gameObject.GetComponent<Renderer>().material = factionColor[0];
                break;
        }
    }

    private void OnDrawGizmos()
    {
        SetFaction();
        ApplyFactionColor();
    }
}
