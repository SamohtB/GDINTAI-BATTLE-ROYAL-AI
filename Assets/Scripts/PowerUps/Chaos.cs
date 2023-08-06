using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaos : PowerUp
{
    public override void TriggerPowerUp(Collider other)
    {
        EventBroadcaster.Instance.PostEvent(EventNames.BRING_CHAOS);
    }
}
