using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : PowerUp
{
    public override void TriggerPowerUp(Collider other)
    {
        EventBroadcaster.Instance.PostEvent(EventNames.MINES_MINES);
    }
}
