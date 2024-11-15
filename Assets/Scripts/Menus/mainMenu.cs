using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.LoadScene("TownScene");
    }

    // Update is called once per frame
    public void Quit()
    {
       
        Application.Quit();
    }
}
