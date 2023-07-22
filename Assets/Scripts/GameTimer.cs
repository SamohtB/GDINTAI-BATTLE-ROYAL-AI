using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float MaxTime = 120.0f;
    [SerializeField] private float timer = 0;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

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

        int minutes = (int)timer / 60;
        int seconds = (int)timer % 60;

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

        if(minutes <= 0 && seconds <= 0)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_GAME_END);
        }
    }
}
