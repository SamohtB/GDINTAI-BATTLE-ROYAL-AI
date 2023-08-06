using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Tank : GridObject
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float defaultSpeed = 1.0f;
    [SerializeField] private float speedUpTimer = 10.0f;
    [SerializeField] private float speedDownTimer = 10.0f;
    [SerializeField] private Faction factionOwner;

    [SerializeField] private bool speedUpOn = false;
    [SerializeField] private bool speedDownOn = false;

    private Coroutine speedUpCoroutine;
    private Coroutine speedDownCoroutine;

    [Header("ReadOnly")]
    [SerializeField] [ReadOnly] private Direction direction;
    [SerializeField] [ReadOnly] private Vector3 offset;

    List<Quaternion> FixedRotations = new List<Quaternion>
    { 
        Quaternion.Euler(90.0f, 0.0f, 0.0f),     //North
        Quaternion.Euler(90.0f, 90.0f, 0.0f),    //East
        Quaternion.Euler(90.0f, 180.0f, 0.0f),   //South
        Quaternion.Euler(90.0f, 270.0f, 0.0f)    //West
    };

    private Vector3 goalNode;
    private Vector3 startNode;
    private bool isMoving = false;
    private float completionRate = 0.0f;

    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }

    public Faction Faction
    {
        get { return factionOwner; }
        set { factionOwner = value; }
    }

    public float MoveSpeed 
    { 
        get { return moveSpeed; } 
        set { moveSpeed = value; } 
    }

    private void Awake()
    {
        direction = Direction.North;
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = 1;
        CurrentHealth = 1;
    }

    public void InitTank(Faction faction)
    {
        factionOwner = faction;
    }

    public void MoveUp()
    {
        if(isMoving) { return; }
        if(direction == Direction.North)
        {
            MoveToDirection(Direction.North);
        }
        else
        {
            direction = Direction.North;
            transform.rotation = FixedRotations[0];
        }
            
    }

    public void MoveDown()
    {
        if(isMoving) { return; }
        if(direction == Direction.South)
        {
            MoveToDirection(Direction.South);
        }
        else
        {
            direction = Direction.South;
            transform.rotation = FixedRotations[2];
        }
    }

    public void MoveLeft()
    {
        if(isMoving) { return; }
        if(direction == Direction.West)
        {
            MoveToDirection(Direction.West);
        }
        else
        {
            direction = Direction.West;
            transform.rotation = FixedRotations[3];
        }
    }

    public void MoveRight()
    {
        if(isMoving) { return; }
        if(direction == Direction.East)
        {
            MoveToDirection(Direction.East);
        }
        else
        {
            direction = Direction.East;
            transform.rotation = FixedRotations[1];
        }
    }

    public void Fire() 
    {
        /* Replace with Object Pool */
        GameObject bullet = Instantiate(bulletPrefab, transform.position + offset, Quaternion.identity);
        bullet.GetComponent<Bullet>().Direction = direction;
        bullet.GetComponent<Bullet>().Faction = Faction;

        Debug.Log("Fire");
    }

    private void MoveToDirection(Direction direction)
    {
        Vector2Int targetGridPosition = PositionOnGrid;

        switch (direction)
        {
            case Direction.North:
                targetGridPosition.y++;
                break;
            case Direction.South:
                targetGridPosition.y--;
                break;
            case Direction.West:
                targetGridPosition.x--;
                break;
            case Direction.East:
                targetGridPosition.x++;
                break;
        }

        Vector2Int bounds = TargetGrid.GetBounds();
        if (targetGridPosition.x < 0 || targetGridPosition.y < 0) { return; }
        if (targetGridPosition.x >= bounds.x || targetGridPosition.y >= bounds.y) { return; }

        if (TargetGrid.CheckWalkable(targetGridPosition))
        {
            StartCoroutine(MoveToPosition(TargetGrid.GetWorldPosition(targetGridPosition)));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        startNode = transform.position;
        goalNode = targetPosition;
        completionRate = 0f;

        while (completionRate < 1f)
        {
            completionRate += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startNode, goalNode, completionRate);
            yield return null;
        }

        isMoving = false;
        PositionOnGrid = TargetGrid.GetGridPosition(transform.position);
    }
    
    public void SpeedUp()
    {
        if(speedUpOn && speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
            Debug.Log("Speed Up Again");
            MoveSpeed = defaultSpeed;
            speedUpCoroutine = null;
        }

        speedUpCoroutine = StartCoroutine(SpeedUpTimer());
    }

    IEnumerator SpeedUpTimer()
    {
        Debug.Log("Speed Up");
        MoveSpeed *= 2;
        speedUpOn = true;

        yield return new WaitForSeconds(speedUpTimer);

        Debug.Log("Speed Up End");
        MoveSpeed = defaultSpeed;
        speedUpOn = false;
        speedUpCoroutine = null;
    }

    public void SpeedDown()
    {
        if(speedDownOn && speedDownCoroutine != null)
        {
            StopCoroutine(speedDownCoroutine);
            Debug.Log("Speed Down Again");
            MoveSpeed = defaultSpeed;
            speedDownCoroutine = null;
        }

        speedDownCoroutine = StartCoroutine(SpeedDownTimer());
    }

    IEnumerator SpeedDownTimer()
    {
        Debug.Log("Speed Down");
        MoveSpeed /= 2;
        speedDownOn = true;

        yield return new WaitForSeconds(speedDownTimer);

        Debug.Log("Speed Down End");
        MoveSpeed = defaultSpeed;
        speedDownOn = false;
        speedDownCoroutine = null;
    }

    public void TakeDamage()
    {
        CurrentHealth--;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if(CurrentHealth <= 0)
        {
            Parameters parameters = new Parameters();
            switch(factionOwner)
            {
                case Faction.HighElf:
                    parameters.PutExtra(EventNames.ON_ELIMINATION, 1); //+1 to dark elf
                    break;

                case Faction.DarkElf:
                    parameters.PutExtra(EventNames.ON_ELIMINATION, 0); //+1 to high elf
                    break;
            }

            EventBroadcaster.Instance.PostEvent(EventNames.ON_ELIMINATION, parameters);
            Destroy(gameObject);
        }
    }

}