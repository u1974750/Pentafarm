using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogClock : MonoBehaviour
{
    private TimeManager tm;
    private LightningManager lm;

    public RectTransform minuteHand;
    public RectTransform hourHand;
    public Image backgroundAfterMidnight;
    public Image backgroundBeforeMidnight;

    private const float hoursToDegrees = 360 / 24;
    private const float minutesToDegrees = 360 / 60;

    void Start()
    {
        tm = FindObjectOfType<TimeManager>();
        lm = FindObjectOfType<LightningManager>();
        Debug.Log(Mathf.Abs(lm.GetNightTime() - 24 / 24));
        backgroundAfterMidnight.fillAmount = lm.GetDayTime() / 24;
        backgroundBeforeMidnight.fillAmount = Mathf.Abs((lm.GetNightTime() - 24 )/ 24);
    }

    void Update()
    {
        hourHand.rotation = Quaternion.Euler(0, 0, -tm.GetHour() * hoursToDegrees);
        minuteHand.rotation = Quaternion.Euler(0, 0, -tm.GetMinute() * minutesToDegrees);
    }
}
