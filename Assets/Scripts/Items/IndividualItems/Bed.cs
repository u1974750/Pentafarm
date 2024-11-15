using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : Interactable
{
    private int AwakeHour;
    private float sleepTimeSpeed = 60f;
    private TimeManager tManager;
    private PlayerController PlayerControllerReference;
    private BlackScreenController screen;
    private bool Sleeping;
    Vector3 previousLocation;
    Quaternion previousRotation;

    private void Start()
    {
        Sleeping = false;
        GameObject bScreen = GameObject.Find("BlackScreen");
        screen= bScreen.GetComponent<BlackScreenController>();
        PlayerControllerReference = GameObject.FindObjectOfType<PlayerController>();
        GameObject tm = GameObject.Find("TimeManager");
        tManager = tm.GetComponent<TimeManager>();
        AwakeHour = (int)GameObject.Find("GameController").GetComponent<LightningManager>().GetDayTime();
    }
    private void OnEnable()
    {
        TimeManager.OnHourChanged += TimeCheck;
    }
    private void OnDisable()
    {
        TimeManager.OnHourChanged -= TimeCheck;
    }
    public override void Interact()
    {
        PlayerControllerReference.setCanClick(false);
        PlayerControllerReference.ChangeAction(PlayerAction.sleep);
        Debug.Log(transform.rotation.eulerAngles);
        Sleeping = true;
        previousLocation = playerTransform.position;
        previousRotation = playerTransform.rotation;
        screen.FadeInAndOut();
        Invoke("Sleep", 1);

    }
    private void TimeCheck()
    {

        if (Sleeping && TimeManager.Hour == AwakeHour)
        {
            tManager.DefaultTimeSpeed();
            screen.FadeInAndOut();
            Invoke("WakeUp", 1);
        }
    }
    private void Sleep()
    {
        PlayerControllerReference.GetComponent<CapsuleCollider>().enabled = false;
        PlayerControllerReference.GetComponent<Rigidbody>().useGravity = false;
        playerTransform.position = new Vector3(transform.position.x, -0.3f, transform.position.z); // Un poco XD, pero bueno
        playerTransform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z);
        PlayerControllerReference.DisableInteraction();
        PlayerControllerReference.anim.SetTrigger("tr_sleep");
        //base.Interact();
        tManager.SpeedUpTime(sleepTimeSpeed);
    }

    private void WakeUp()
    {
        PlayerControllerReference.anim.SetTrigger("tr_getUp");
        PlayerControllerReference.EnableInteraction();
        Sleeping = false;
        PlayerControllerReference.GetComponent<Rigidbody>().useGravity = true;
        PlayerControllerReference.GetComponent<CapsuleCollider>().enabled = true;
        playerTransform.position = previousLocation;
        playerTransform.rotation = previousRotation;
        PlayerControllerReference.ActionFinish();
    }
}
