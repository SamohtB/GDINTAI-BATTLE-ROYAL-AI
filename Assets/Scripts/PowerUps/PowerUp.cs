using UnityEngine;

public class PowerUp : GridObject
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK")
        {
            TriggerPowerUp(other);
            RemoveFromGrid(other.transform.position);
        }
        else
        {
            Debug.Log("NON TANK COLLISION!");
        }
        
        Destroy(gameObject);
    }

    public virtual void TriggerPowerUp(Collider other) {}
}
