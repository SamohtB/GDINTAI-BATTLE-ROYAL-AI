using UnityEngine;

public class PowerUp : GridObject
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK")
        {
            TriggerPowerUp(other);
            RemoveFromGrid(transform.position);

        }
        else
        {
            Debug.Log("NON TANK COLLISION!");
        }
        
        gameObject.SetActive(false);
    }


    public virtual void TriggerPowerUp(Collider other) {}
}
