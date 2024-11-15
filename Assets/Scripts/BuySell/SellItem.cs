using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellItem : MonoBehaviour
{
    public GameObject moneyObj;
    public AudioClip soundSell;
    private TextMeshProUGUI textMoneyUI;
    private bool isColliding = false;

    private AudioSource audio;

    void Start()
    {
        textMoneyUI = moneyObj.GetComponent<TextMeshProUGUI>();
        ActualizeMoneyUI();
        audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isColliding)
        {
            if (Input.GetMouseButtonDown(0)) //CAMBIAR A MOUSEBUTTON
            {
                ItemDetails activeItem = InventoryManager.Instance.GetItemActive();
                if (activeItem != null)
                {
                    if (activeItem.canBeSold)
                    {                       
                        audio.PlayOneShot(soundSell);
                        InventoryManager.Instance.AddItem(InventoryLocation.recoverShop, activeItem.itemCode);
                        InventoryManager.Instance.RemoveItem(InventoryLocation.player, activeItem.itemCode);
                        SellItemSelected(activeItem.itemCode);
                        ActualizeMoneyUI();
                        //InventoryManager.Instance.DebugPrintInventoryList();
                        //Debug.Log("precio"+activeItem.sellPrice);
                    }
                }
            }
        }
    }

    /// <summary>
    /// intenta vender item indicado
    /// </summary>
    /// <param name="item">item</param>
    public void SellItemSelected(int item)
    {
        int price = InventoryManager.Instance.GetPriceBuy(item);
        if (price != -1)
        {
            InventoryManager.Instance.AddMoney(price);
            InventoryManager.Instance.AddItem(InventoryLocation.recoverShop, item);
            ActualizeMoneyUI();
            //InventoryManager.Instance.DebugPrintInventoryList();
            //int cartera = InventoryManager.Instance.GetMoneyInInventory();
            //Debug.Log("Cartera: "+cartera);
        }
    }

    private void ActualizeMoneyUI()
    {
        textMoneyUI.text = (InventoryManager.Instance.GetMoneyInInventory()).ToString();
        //textMoneyUI.text = (InventoryManager.Instance.GetMoneyInInventory()).ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}

