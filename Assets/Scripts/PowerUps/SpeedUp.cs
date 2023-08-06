using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : PowerUp
{
    public override void TriggerPowerUp(Collider other) 
    {
        other.GetComponent<Tank>().SpeedUp();
    }
}
