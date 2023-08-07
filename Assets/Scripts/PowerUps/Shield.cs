using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Shield : PowerUp
{
    public override void TriggerPowerUp(Collider other)
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
