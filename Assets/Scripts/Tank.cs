using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using static Enum;

public class Tank : MonoBehaviour
{
    
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private float moveSpeed = 100.0f;
    [SerializeField] private float defaultSpeed = 100.0f;
    [SerializeField] private float speedUpTimer = 10.0f;
    [SerializeField] private float speedDownTimer = 10.0f;
    [SerializeField] private Faction factionOwner;

    [SerializeField] private bool speedUpOn = false;
    [SerializeField] private bool speedDownOn = false;

    private Coroutine speedUpCoroutine;
    private Coroutine speedDownCoroutine;

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

        Debug.Log("Speed Up End");
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
