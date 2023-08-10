using UnityEngine;

public class PlayerTank : Tank
{
    private void Update()
    {
        ticks += Time.deltaTime;
        if(IsAlive)
            PlayerShoot();
    }

    private void FixedUpdate()
    {
        if(IsAlive)
            PlayerMovement();
    }

    void PlayerShoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    void PlayerMovement()
    {
        float movement_z = Input.GetAxis("Vertical");
        float movement_x = Input.GetAxis("Horizontal");

        if (movement_z > 0)
        {
            MoveUp();
        }

        else if (movement_z < 0)
        {
            MoveDown();
        }

        if (movement_x < 0)
        {
            MoveLeft();   
        }

        else if (movement_x > 0)
        {
            MoveRight();
        }
    }

    
}


