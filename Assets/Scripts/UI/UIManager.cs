using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    private bool _pauseMenuOn = false;

    public QuitMenuManager QuitMenuManager;
    [SerializeField] private GameObject pauseInventoryObject = null;
    [SerializeField] private PauseMenuInventoryManagement pauseMenuInventoryManagement = null;
    [SerializeField] private GameObject moneyObject = null;
    [SerializeField] private GameObject inventoryObject = null;
    [SerializeField] private GameObject clockObject = null;


    /// <summary>
    /// Define si el juego esta o no en pausa
    /// </summary>
    public bool PauseMenuOn { get { return _pauseMenuOn; } set { _pauseMenuOn = value; } }

    protected override void Awake()
    {
        base.Awake();
        pauseInventoryObject.SetActive(false);
    }
    private void Update()
    {
        PauseMenu();     
    }

    private void PauseMenu()
    {
        if(Input.GetKeyUp(KeyCode.E) && QuitMenuManager.QuitMenuOn == false)
        {
            if(PauseMenuOn) { DisablePauseMenu(); }
            else { EnablePauseMenu();}
        }
        if(QuitMenuManager.QuitMenuOn == true)
        {
            if (PauseMenuOn) { DisablePauseMenu(); }
        }
    }

    private void EnablePauseMenu()
    {
        PauseMenuOn = true;
        //inmovilizar jugador (variable PlayerInputIsDisabled por ejemplo)        
        Time.timeScale = 0; //pausar tiempo

        pauseInventoryObject.SetActive(true);
        inventoryObject.SetActive(false);
        System.GC.Collect();

    }

    private void DisablePauseMenu()
    {
       // pauseMenuInventoryManagement.DestroyCurrentlyDraggedItems();
        PauseMenuOn = false;
        //volver a movilizar al jugador
        Time.timeScale = 1;
        pauseInventoryObject.SetActive(false);
        inventoryObject.SetActive(true);
    }
}
