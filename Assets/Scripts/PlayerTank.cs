using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : Tank
{
    [Header("Tank Fire Cooldown")]
    [SerializeField] private float shotCooldown = 1.0f;
    [SerializeField] private float shotTimer = 1.0f;
    [SerializeField] [ReadOnly] private Vector3 offset;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private Rigidbody rb;
    [SerializeField] [ReadOnly] private Enum.Direction direction;
    

    List<Quaternion> FixedRotations = new List<Quaternion>
    { 
        Quaternion.Euler(90.0f, 0.0f, 0.0f),     //North
        Quaternion.Euler(90.0f, 90.0f, 0.0f),    //East
        Quaternion.Euler(90.0f, 180.0f, 0.0f),   //South
        Quaternion.Euler(90.0f, 270.0f, 0.0f)    //West
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        direction = Enum.Direction.North;
    }


    private void Update()
    {
        PlayerShoot();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerShoot()
    {
        shotTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && shotTimer <= 0)
        {
            shotTimer = shotCooldown;
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.identity);
            bullet.GetComponent<Bullet>().Direction = direction;
            bullet.GetComponent<Bullet>().Faction = Faction;

            Debug.Log("Fire");
        }
    }

    void PlayerMovement()
    {
        float movement_z = Input.GetAxis("Vertical");
        float movement_x = Input.GetAxis("Horizontal");
  

        if (movement_z > 0)
        {
            if (direction == Enum.Direction.North) // Up
            {
                movement_z *= Time.deltaTime;
            }
            else // Not moving North
            {
                direction = Enum.Direction.North;
                movement_z = 0.0f;
                transform.rotation = FixedRotations[0];
            }
        }

        else if (movement_z < 0)
        {
            if (direction == Enum.Direction.South) // Down
            {
                movement_z *= Time.deltaTime;
            }
            else // Not moving South
            {
                direction = Enum.Direction.South;
                movement_z = 0.0f;
                transform.rotation = FixedRotations[2];
            }
        }

        if (movement_x < 0)
        {
            if (direction == Enum.Direction.West) // Left
            {
                movement_x *= Time.deltaTime;
            }
            else // Not moving West
            {
                direction = Enum.Direction.West;
                movement_x = 0.0f;
                transform.rotation = FixedRotations[3];
            }
        }
        else if (movement_x > 0)
        {
            if (direction == Enum.Direction.East) // Right
            {
                movement_x *= Time.deltaTime;
            }
            else // Not moving East
            {
                direction = Enum.Direction.East;
                movement_x = 0.0f;
                transform.rotation = FixedRotations[1];
            }
        }

        rb.velocity = new Vector3(movement_x, 0, movement_z) * MoveSpeed;
    }
}


