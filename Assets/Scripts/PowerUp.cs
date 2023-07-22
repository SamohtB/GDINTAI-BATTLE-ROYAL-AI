using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private PowerUpType powerUpType;

    public PowerUpType PowerUpType 
    {
        get {  return powerUpType; }
        set { powerUpType = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK")
        {
            Debug.Log("Power UP! " + PowerUpType.ToString());

            switch(PowerUpType)
            {
                case PowerUpType.SpeedUp:
                    SpeedUpTank(other);
                    break;

                case PowerUpType.SpeedDown:
                    SpeedDownTank(other);
                    break;

                case PowerUpType.BaseShield:
                    ShieldUp(other);
                    break;

                case PowerUpType.Mines:
                    //Broadcast Mines
                    break;

                case PowerUpType.Chaos:
                    //Broadcast Chaos
                    break;

                default:
                    Debug.Log("NULL POWER UP");
                    break;
            }
        }
        else
        {
            Debug.Log("NON TANK COLLISION!");
        }
        
        Destroy(gameObject);
    }

    private static void SpeedDownTank(Collider other)
    {
        if (other.GetComponent<Tank>() != null)
        {
            other.GetComponent<Tank>().SpeedDown();
        }
        else
        {
            Debug.LogError("NOT A TANK. COMPONENT ERROR!");
        }
    }

    private static void SpeedUpTank(Collider other)
    {
        if (other.GetComponent<Tank>() != null)
        {
            other.GetComponent<Tank>().SpeedUp();
        }
        else
        {
            Debug.LogError("NOT A TANK. COMPONENT ERROR!");
        }
    }

    private static void ShieldUp(Collider other)
    {
        if (other.GetComponent<Tank>() != null)
        {
            Faction faction = other.GetComponent<Tank>().Faction;
            Parameters parameters = new Parameters();

            switch(faction)
            {
                case Faction.HighElf:
                    parameters.PutExtra(EventNames.SHIELDS_UP, 0);
                    break;

                case Faction.DarkElf:
                    parameters.PutExtra(EventNames.SHIELDS_UP, 1);
                    break;

                default:
                    Debug.LogError("Shield Power Up Error!");
                    break;
            }

            EventBroadcaster.Instance.PostEvent(EventNames.SHIELDS_UP, parameters);
        }
    }
}
