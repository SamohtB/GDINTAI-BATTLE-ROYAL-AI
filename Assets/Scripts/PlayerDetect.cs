using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class PlayerDetect : MonoBehaviour
{
    public bool PlayerIsDetected { get; private set; }

    private void Start()
    {
        PlayerIsDetected = false;
    }

    private void OnTriggerEnter(Collider other)
    { 
        if(other.tag == "TANK" && other.gameObject != gameObject)
        {
            PlayerIsDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "TANK" && other.gameObject != gameObject)
        {
            PlayerIsDetected = false;
        }
    }
}
