using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class belial_controller : MonoBehaviour
{
    public GameObject pointStart;   //0
    public GameObject pointMiddle; // 1
    public GameObject pointEnd;    // 2

    private bool move = false;
    private bool back = true;
    private int currentPos = 0;
    private int rotationSpeed = 150;
    private float speed = 6;
    private Animator anim;
    private GameObject nextPoint;

    private void Start()
    {
        anim = GetComponent<Animator>();
        nextPoint = pointMiddle;
    }

    private void Update()
    {
        if (move)
        {
            anim.SetBool("walk", true);

            Vector3 direction = nextPoint.transform.position;
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            if (currentPos == 0){
                transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;
                nextPoint = pointMiddle;
            }
            else if(currentPos == 1 && !back){
                transform.position += new Vector3(0f, 0f, speed) * Time.deltaTime;
                nextPoint = pointEnd;
            }
            else if(currentPos == 1 && back){
                transform.position += new Vector3(-speed, 0f, 0f) * Time.deltaTime;
                nextPoint = pointStart;
            }
            else if(currentPos == 2){
                transform.position += new Vector3(0f, 0f, -speed) * Time.deltaTime;
                nextPoint = pointMiddle;
            }
            
            if(transform.position.x >= pointMiddle.transform.position.x && !back && currentPos != 1)
            {
                currentPos = 1;               
            }  
            else if(transform.position.z <= pointMiddle.transform.position.z && back && currentPos != 1)
            {
                Debug.Log("IF");
                currentPos = 1;
                
            }
            else if(transform.position.z >= pointEnd.transform.position.z && !back)
            {
                move = false;
                anim.SetBool("walk", false);
                currentPos = 2;
            }
            else if(transform.position.x <= pointStart.transform.position.x && back)
            {
                move = false;
                anim.SetBool("walk", false);
                currentPos = 0;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!move)
            {
                move = true;
                back = !back;
            }
        }
    }
}
