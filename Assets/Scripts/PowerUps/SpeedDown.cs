using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDown : PowerUp
{
    
    public override void TriggerPowerUp(Collider other)
    {
        {
            other.GetComponent<Tank>().SpeedDown();
        }
    }
}
