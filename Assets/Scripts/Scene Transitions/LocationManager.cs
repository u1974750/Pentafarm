using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    public List<StartPoint> startPoints;

    private void Awake()
    {
        // If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            // Set the static instance to this instance
            Instance = this;
        }
    }

    // Find the player's start position based on where he's coming from
    public Transform GetPlayerStartingPosition(SceneTransitionManager.Location enteringFrom)
    {
        // Tries to find the matching startpoint based on the location given 
        StartPoint startingPoint = startPoints.Find(x => x.enteringFrom == enteringFrom);

        // Returns the transform
        return startingPoint.playerStart;
    }
}
