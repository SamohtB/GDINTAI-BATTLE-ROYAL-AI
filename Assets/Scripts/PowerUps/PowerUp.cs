using UnityEngine;

public class PowerUp : GridObject
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK")
        {
            TriggerPowerUp(other);
            RemoveFromGrid(transform.position);
            gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("NON TANK COLLISION!");
        }
    }


    public virtual void TriggerPowerUp(Collider other) {}
}
