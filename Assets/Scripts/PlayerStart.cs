using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private Faction factionOwner;
    [SerializeField] private int baseNumber;
    [SerializeField] private float shieldTimer = 10.0f;
    [SerializeField] private bool shieldsUp = false;

    private Coroutine shieldCoroutine = null;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.SHIELDS_UP, this.ShieldBase);    
    }

    public Faction Faction
    {
        get { return factionOwner; }
        set { factionOwner = value; }
    }

    public GameObject CreateTank()
    {
        GameObject tank = Instantiate(tankPrefab, this.transform);
        tank.GetComponent<Tank>().InitTank(factionOwner);

        return tank;
    }

    public void ShieldBase(Parameters param)
    {
        int faction_shield = param.GetIntExtra(EventNames.SHIELDS_UP, -1);

        if(faction_shield == (int)Faction)
        {
            if(shieldsUp && shieldCoroutine != null)
            {
                StopCoroutine(shieldCoroutine);
                Debug.Log("Shield Up Again");
                shieldCoroutine = null;
            }

            StartCoroutine(ShieldsUp());
        }
        else if(faction_shield == -1)
        {
            Debug.LogError("Shield Broadcast Error!");
        }
    }

    IEnumerator ShieldsUp()
    {
        Debug.Log("Shield Power Up On!" + gameObject.name);
        shieldsUp = true;
     
        yield return new WaitForSeconds(shieldTimer);

        Debug.Log("Shield Power Up Off!" + gameObject.name);
        shieldsUp = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK" && other.GetComponent<Tank>().Faction != Faction)
        {
            Debug.Log(gameObject.name + " Destroyed!");

            Parameters parameters = new Parameters();

            switch(factionOwner)
            {
                case Faction.HighElf:
                    parameters.PutExtra(EventNames.ON_BASE_DESTROYED, 0); //--1 to high elf
                    break;

                case Faction.DarkElf:
                    parameters.PutExtra(EventNames.ON_BASE_DESTROYED, 1); //--1 to dark elf
                    break;
            }

            EventBroadcaster.Instance.PostEvent(EventNames.ON_BASE_DESTROYED, parameters);

            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
        }
    }
}
