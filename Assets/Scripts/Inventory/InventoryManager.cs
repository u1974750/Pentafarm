using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    [SerializeField] private SO_itemList itemList = null;
    private Dictionary<int, ItemDetails> itemDetailsDictionary; //Diccionario de items -> inventario
    private int selectedInventoryItem = 0;
    private int moneyInInventory;

    [HideInInspector] public int[] inventoryListCapacityInArray; //
    public List<InventoryItem>[] inventoryLists; //array de listas de inventarios
    public GameObject itemBarObj;
    public int SelectedItem {
        get { return selectedInventoryItem; }
        set { selectedInventoryItem = value; }
    }



    protected override void Awake()
    {
        base.Awake();
        moneyInInventory = 1000;

        CreateInventoryList(); //crea listas de inventario        
        CreateItemDetailsDictionary();//Crea diccionario de items

    }

    //PRIVATE
    private void CreateInventoryList()
    {
        inventoryLists = new List<InventoryItem>[(int)InventoryLocation.count]; //0 = player ;; 1 = cofre

        for (int i = 0; i < (int)InventoryLocation.count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }
        inventoryListCapacityInArray = new int[(int)InventoryLocation.count];
        inventoryListCapacityInArray[(int)InventoryLocation.player] = PlayerController.playerMaximumInventoryCapacity;

    }

    /// <summary>
    ///  Llena ItemDetailDictionary del script de objetos Scriptables (SO_itemList)
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();

        foreach (ItemDetails itemDetails in itemList.itemDetails)
        {
            itemDetailsDictionary.Add(itemDetails.itemCode, itemDetails);
        }
    }

    /// <summary>
    /// Devuelve pos en inventario del objeto, -1 en caso que no exista
    /// </summary>
    /// <param name="inventoryLocation">inventario a comprovar; 0 = player, 1 = cofre</param>
    /// <param name="itemCode">codigo identificador del objeto a buscar</param>
    /// <returns></returns>
    private int FindItemInInventory(InventoryLocation inventoryLocation, int itemCode)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemCode == itemCode)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Añade item al final del inventario
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode)
    {
        InventoryItem inventoryItem = new InventoryItem();

        inventoryItem.itemCode = itemCode;
        inventoryItem.itemQuantity = 1;
        inventoryItem.isNull = false;

        bool founded = false;
        int i = 0;
        while(!founded && i < inventoryList.Count)
        {
            if (inventoryList[i].isNull)
                founded = true;
            else
                i++;            
        }
        if (founded)
            inventoryList[i] = inventoryItem;
        else
            inventoryList.Add(inventoryItem);

    }

    /// <summary>
    /// Añade item a pos position
    /// </summary>
    private void AddItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();

        int quantity = inventoryList[position].itemQuantity + 1;
        inventoryItem.itemQuantity = quantity;
        inventoryItem.itemCode = itemCode;
        inventoryItem.isNull = false;
        inventoryList[position] = inventoryItem;

    }
    
    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemCode, int position)
    {
        InventoryItem inventoryItem = new InventoryItem();
        int quantity = inventoryList[position].itemQuantity - 1;

        if(quantity > 0)
        {           
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemCode = itemCode;
            inventoryList[position] = inventoryItem;
        }
        else
        {
            if (position != inventoryList.Count - 1)
            {
                inventoryItem.isNull=true;
                inventoryItem.itemCode = 9999;
                inventoryList[position] = inventoryItem;
            }
            else
            {
                inventoryList.RemoveAt(position);
            }
        }
    }


    //PUBLIC

    /// <summary>
    /// Obtener la descripcion para el tipo de item
    /// </summary>
    /// <param name="itemType">variable itemType del item</param>
    /// <returns>string con el tipo de item</returns>
    public string GetItemTypeDescription(ItemType itemType)
    {
        string itemTypeDescription;
        switch (itemType)
        {
            case ItemType.Hoeing_tool:
                itemTypeDescription = "Hoe";
                break;
            case ItemType.Chopping_tool:
                itemTypeDescription = "Axe";
                break;
            case ItemType.Breaking_tool:
                itemTypeDescription = "Pickaxe";
                break;
            case ItemType.Reaping_tool:
                itemTypeDescription = "Scythe";
                break;
            case ItemType.Watering_tool:
                itemTypeDescription = "Watering Can";
                break;
            default:
                itemTypeDescription = itemType.ToString();
                break;

        }
        return itemTypeDescription;
    }

    ///<summary>
    /// Devuelve los itemDetails del itemCode indicado, null si no existe el item
    ///</summary>
    public ItemDetails GetItemDetails(int itemCode)
    {
        ItemDetails itemDetails;

        if (itemDetailsDictionary.TryGetValue(itemCode, out itemDetails))
        {
            return itemDetails;
        }
        else {
            return null;
        }
    }

    /// <summary>
    /// Añade item al inventario seleccionado
    /// </summary>
    /// <param name="inventoryLocation">tipo de inventario, 0 - Jugador, 1 - Cofre</param>
    /// <param name="item"> Item a añadir al inventario seleccionado</param>
    public void AddItem(InventoryLocation inventoryLocation, int itemCode)
    {
        //int itemCode = item.ItemCode;
        List<InventoryItem> inventoryList = inventoryLists[(int)inventoryLocation];
        //mirem si l'inventari ja te l'item
        int itemPoition = FindItemInInventory(inventoryLocation, itemCode);

        if (itemPoition != -1)
        {
            AddItemAtPosition(inventoryList, itemCode, itemPoition);
        }
        else
        {
            AddItemAtPosition(inventoryList, itemCode);
        }
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    public void RemoveItem(InventoryLocation inventoryLocation, int itemCode)
    {
        int itemPosition = FindItemInInventory(inventoryLocation, itemCode);
        if (itemPosition != -1)
        {
            RemoveItemAtPosition(inventoryLists[(int)inventoryLocation], itemCode, itemPosition);
        }
        EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
    }

    public void SwapInventoryItems(InventoryLocation inventoryLocation, int fromItem, int toItem)
    {
        if(fromItem < inventoryLists[(int)inventoryLocation].Count && toItem < inventoryLists[(int)inventoryLocation].Count && fromItem != toItem && fromItem >= 0 && toItem >= 0)
        {
            InventoryItem fromInventoryItem = inventoryLists[(int)inventoryLocation][fromItem];
            InventoryItem toInventoryItem = inventoryLists[(int)inventoryLocation][toItem];

            inventoryLists[(int)inventoryLocation][toItem] = fromInventoryItem;
            inventoryLists[(int)inventoryLocation][fromItem] = toInventoryItem;

            EventHandler.CallInventoryUpdatedEvent(inventoryLocation, inventoryLists[(int)inventoryLocation]);
            
        }
    }

    /// <summary>
    /// Devuelve precio de vender el objeto
    /// </summary>
    /// <param name="itemCode">Codigo del item a conocer precio</param>
    /// <returns>int precio, -1 si no existe tal objeto</returns>
    public int GetPriceSold(int itemCode)
    {
        ItemDetails itemDetails = GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            int price = itemDetails.sellPrice;
            return price;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Devuelve precio de comprar el objeto
    /// </summary>
    /// <param name="itemCode">Item del item a conocer</param>
    /// <returns>precio del item, -1 si item no existe</returns>
    public int GetPriceBuy(int itemCode)
    {
        ItemDetails itemDetails = GetItemDetails(itemCode);
        if (itemDetails != null)
        {
            int price = itemDetails.buyPrice;
            return price;
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Añade cantidad de dinero indicada al monedero
    /// </summary>
    /// <param name="money">cantidad dinero a añadir (>0) </param>
    public void AddMoney(int money)
    {
        if (money > 0)
            moneyInInventory += money;
    }

    /// <summary>
    /// Quita cantidad de dinero indicada del monedero en caso que tenga esa cantidad de dinero
    /// </summary>
    /// <param name="money">cantidad de dinero a extraer</param>
    /// <returns>true si ha podido sacar dinero, false si dinero insuficiente</returns>
    public bool RemoveMoney(int money)
    {
        if (money > 0 && money <= moneyInInventory)
        {
            moneyInInventory -= money;
            return true;
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// Getter del dinero actual en monedero
    /// </summary>
    /// <returns>int dinero</returns>
    public int GetMoneyInInventory()
    {
        return moneyInInventory;
    }

    /// <summary>
    /// Obtener DetallesItem que se tiene seleccionado en el inventario
    /// </summary>
    /// <returns>ItemDetails del ojeto seleccionado, null en caso que no contenga item</returns>
    public ItemDetails GetItemActive()
    {
        List<InventoryItem> inv = inventoryLists[(int)InventoryLocation.player];
        if (selectedInventoryItem < inv.Count)
        {
            int itemCode = inv[selectedInventoryItem].itemCode;
            ItemDetails itemToReturn = GetItemDetails(itemCode);
            return itemToReturn;

        }
        return null;
    }
    
    /// <summary>
    /// Fuerza la actualizacion del inventario pequeño del jugador
    /// </summary>
    public void UpdateInventoryPlayer()
    {
        EventHandler.CallInventoryUpdatedEvent(InventoryLocation.player, inventoryLists[(int)InventoryLocation.player]);
    }
    
    
    public void DebugPrintInventoryList()
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)InventoryLocation.recoverShop];
        foreach (InventoryItem inventoryItem in inventoryList)
        {
            Debug.Log("NOMBRE ITEM: "
                + InventoryManager.Instance.GetItemDetails(inventoryItem.itemCode).itemDescription
                + "   CANTIDAD ITEM: " + inventoryItem.itemQuantity);
            Debug.Log("***");
        }
    }
}

