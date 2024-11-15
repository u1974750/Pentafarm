using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location
    {
        TownScene,
        ShopScene,
        SampleScene,
        MainMenu
    }

    public Location currentLocation;

    private Transform playerPoint;


    private void Awake()
    {
        // If there is more than 1 instance, destroy GameObject
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Make the gameobject persistent across scenes
        DontDestroyOnLoad(gameObject);

        // OnLocationLoad will be called when the scene is loaded
        SceneManager.sceneLoaded += OnLocationLoad;

        // Find the player's transform
        playerPoint = FindObjectOfType<PlayerController>().transform;

    }

    // Switch the player to another scene
    public void SwitchLocation(Location locationToSwitch)
    {
        SceneManager.LoadScene(locationToSwitch.ToString());
    }

    // Called when the scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        // The location the player is coming from when the scene loads
        Location oldLocation = currentLocation;

        // Get the new location by converting the string of our current scene into a Location enum value
        Location newLocation = (Location) System.Enum.Parse(typeof(Location), scene.name);

        // If the player is not coming from any new place, stop executing the function
        if (currentLocation == newLocation) return;

        if (playerPoint != null)
        {
            // Find the start point
            Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

            // Disable character controller component
            PlayerController playerCharacter = playerPoint.GetComponent<PlayerController>();
            playerCharacter.enabled = false;

            // Change the player's position and rotation to the start point
            playerPoint.SetPositionAndRotation(startPoint.position, startPoint.rotation);

            // Re-enable character controller component
            playerCharacter.enabled = true;
        }


        // Save the current location that we just switched to
        currentLocation = newLocation;
    }
}
