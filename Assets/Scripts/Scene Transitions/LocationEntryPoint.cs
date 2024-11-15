using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to player
        if (other.CompareTag("Player"))
        {
            // Switch scenes
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }
    }
}
