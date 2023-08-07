using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Mine : GridObject
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK")
        {
            Debug.Log(other.name + "MINED!");

            if (other.GetComponent<Tank>() != null)
            {
               other.GetComponent<Tank>().TakeDamage();
               gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("NOT A TANK. COMPONENT ERROR!");
            }
        }
    }
}
