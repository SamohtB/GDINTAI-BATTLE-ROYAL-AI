using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerTank : Tank
{

    private void Update()
    {
        PlayerShoot();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerShoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Fire();
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


