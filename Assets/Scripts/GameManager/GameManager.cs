using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float MaxTime = 120.0f;
    [SerializeField] private float timer = 0;

    void Start()
    {
        timer = MaxTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if((int)timer > 0)
        {
            Countdown();
        }
    }

    private void Countdown()
    {
        timer -= Time.deltaTime;

        int minutes = Mathf.CeilToInt(timer) / 60;
        int seconds = Mathf.CeilToInt(timer) % 60;

        string time;

        if (seconds < 10)
        {
            time = minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            time = minutes.ToString() + ":" + seconds.ToString();
        }

        timerText.text = time;

        if(timer <= 0)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_GAME_END);
        }
    }

    public float GetGameTime()
    {
        return timer;
    }
    
}
