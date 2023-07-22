using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 600.0f;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private Faction factionOwner;
    [SerializeField] [ReadOnly] private Direction direction;
    [SerializeField] [ReadOnly] private Rigidbody rb;

    List<Vector3> moveDir = new List<Vector3>
    {
        new Vector3(1f, 0f, 0f),    //North
        new Vector3(-1f, 0f, 0f),   //South
        new Vector3(0f, 0f, 1f),    //East
        new Vector3(0f, 0f, -1f)    //West
    };

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    public Faction Faction
    {
        get { return factionOwner; }
        set { factionOwner = value; }
    }

    public Direction Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    void Update()
    {
        switch (Direction)
        {
            case Direction.North:
                rb.velocity = moveDir[2] * moveSpeed;
                break;
            case Direction.South:
                rb.velocity = moveDir[3] * moveSpeed;
                break;
            case Direction.East:
                rb.velocity = moveDir[0] * moveSpeed;
                break;
            case Direction.West:
                rb.velocity = moveDir[1] * moveSpeed;
                break;
            default:
                rb.velocity = Vector3.zero; // Stop the bullet if the direction is not set correctly.
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TANK" && other.GetComponent<Tank>().Faction != Faction)
        {
            Debug.Log(other.name + "HIT!");

            if (other.GetComponent<Tank>() != null)
            {
               other.GetComponent<Tank>().TakeDamage();
               Destroy(gameObject);
            }
            else
            {
                Debug.LogError("NOT A TANK. COMPONENT ERROR!");
            }
        }
        else if(other.tag == "WALL")
        {
            Debug.Log("Wall Hit");
            Destroy(gameObject);
        }
    }
}
