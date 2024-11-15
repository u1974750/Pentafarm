using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public GameObject moneyObj;
    private TextMeshProUGUI textMoneyUI;

    private void Start()
    {
        textMoneyUI = moneyObj.GetComponent<TextMeshProUGUI>();
        ActualizeMoneyUI();
    }

    /// <summary>
    /// intenta comprar item indicado
    /// </summary>
    /// <param name="item">item</param>
    public void BuyItemSelected(Item item)
    {
        //ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
        int price = InventoryManager.Instance.GetPriceBuy(item.ItemCode);
        if (InventoryManager.Instance.RemoveMoney(price))
        {
            InventoryManager.Instance.AddItem(InventoryLocation.player, item.ItemCode);
            ActualizeMoneyUI();
        }
    }

    private void ActualizeMoneyUI()
    {
        textMoneyUI.text = (InventoryManager.Instance.GetMoneyInInventory()).ToString();
        //textMoneyUI.text = (InventoryManager.Instance.GetMoneyInInventory()).ToString();
    }

}


//START INICIALIZA al primer frame
//UPDATE SE EJECUTA CADA FRAME
//AWAKE SOLO SE EJECUTA ANTES DE EMPEZAR EL PRIMER FRAME
