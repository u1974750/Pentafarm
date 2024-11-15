using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuitMenuManager : MonoBehaviour
{
    public static bool QuitMenuOn = false;
    public GameObject QuitMenuUI;
    public GameObject PlayerController;


    void Awake()
    {
        Time.timeScale = 1;

        QuitMenuUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){

            if (QuitMenuOn) Resume();
            else Pause();
        } 
    }
    public void Resume()
    {
        Time.timeScale = 1;
        QuitMenuUI.SetActive(false);
        PlayerController.SetActive(true);

        QuitMenuOn = false;
    }
    void Pause()
    {
        Time.timeScale = 0;
        QuitMenuUI.SetActive(true);
        PlayerController.SetActive(false);

        QuitMenuOn = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
