using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static Action OnDayChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }

    [SerializeField] private int GameDayInRealSeconds = 120;
    [SerializeField] private int minuteIncrement = 1;
    private const int WAKE_UP_HOUR = 10;
    private float minuteToRealTime;
    private float timer;
    private float speedIncrement = 1;


    // Start is called before the first frame update
    void Start()
    {
        Minute = 0;
        Hour = WAKE_UP_HOUR;
        Day = 1;
        timer = 0;
        minuteToRealTime = ((float)GameDayInRealSeconds * minuteIncrement) / 1440f;
    }

    // Update is called once per frame
    void Update()
    {
        // Video https://www.youtube.com/watch?v=Y_AOfPupWhU

        timer += Time.deltaTime;

        if(timer >= minuteToRealTime)
        {
            Minute+= minuteIncrement * (int)speedIncrement;
            OnMinuteChanged?.Invoke();

            if(Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChanged?.Invoke();
            }
            if (Hour >= 24)
            {
                Day++;
                Hour = 0;
                OnDayChanged?.Invoke();
            }

            timer -= minuteToRealTime;
            // Debug.Log("Hour: " + Hour + ", Minute: " + Minute);
        }
    }

    public void SpeedUpTime(float SpeedIncrement)
    {
        speedIncrement = SpeedIncrement;
    }
    public void setHour(int newHour = WAKE_UP_HOUR)
    {
        Hour = newHour;
    }

    public void DefaultTimeSpeed()
    {
        speedIncrement= 1;
    }
    
    public float GetHour()
    {
        return (float)Hour + (float)Minute/60 ;
    }
    public float GetMinute()
    {
        return (float)Minute;
    }
    public float GetDay()
    {
        return Day;
    }
}
