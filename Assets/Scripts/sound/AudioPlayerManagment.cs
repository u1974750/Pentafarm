using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerManagment : MonoBehaviour
{
    [Space(5)]
    [Header("Audio Sources")]
    public AudioSource audio;
    public AudioSource steps;
    public AudioSource music;

    [Space(5)]
    [Header("Clips")]
    //Clips
    public AudioClip TakeItem;
    public AudioClip PlantSeed;
    public AudioClip Watering;
    public AudioClip Hoging;
    public AudioClip Build;
    public AudioClip Demolition;
    public AudioClip Buy;
    public AudioClip CollectPlant;
    public AudioClip Walk;
    public AudioClip Run;
    public AudioClip Sell;


    //privado
    bool isStepRun = false;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void playTakeItem()
    {       
        audio.PlayOneShot(TakeItem);
    }
    public void playPlantSeed()
    {
        audio.PlayOneShot(PlantSeed);
    }

    public void playWatering()
    {
        audio.PlayOneShot(Watering);
    }
    public void playHoging()
    {
        audio.PlayOneShot(Hoging);
    }
    public void playBuild()
    {
        audio.PlayOneShot(Build);
    }
    public void playDemolition()
    {
        audio.PlayOneShot(Demolition);
    }
    public void playBuy()
    {
        audio.PlayOneShot(Buy);
    }
    public void playSell()
    {
        audio.PlayOneShot(Sell);
    }
    public void playCollectPlant()
    {
        audio.PlayOneShot(CollectPlant);
    }
    public void playWalk()
    {
        if (!steps.isPlaying || isStepRun)
        {
            //Debug.Log("Walk");
            isStepRun = false;
            steps.Stop();
            steps.clip = Walk;
            steps.Play();
        }        
    }
    public void playRun()
    {

        if(!steps.isPlaying || !isStepRun)
        {
           // Debug.Log("Run");
            isStepRun = true;
            steps.Stop();
            steps.clip = Run;
            steps.Play();
        }       
    }

    public void stopSteps()
    {
        if (steps.isPlaying)
        {
           // Debug.Log("Stop");
            steps.Stop();
        }
    }

    
}
