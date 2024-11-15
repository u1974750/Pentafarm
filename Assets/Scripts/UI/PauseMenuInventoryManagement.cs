using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInventoryManagement : MonoBehaviour
{
    [SerializeField] private PauseMenuInventoryManagementSlot[] inventoryManagementSlot = null;
    public GameObject inventoryManagementDraggedItemPrefab;
    [SerializeField] private Sprite transparent16x16 = null;
    [HideInInspector] public GameObject inventoryTextBoxGameObject;

    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += PopulatePlayerInventory;
        if(InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.player, InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player]);
        }
    }

    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= PopulatePlayerInventory;
        DestroyInventoryTextBoxGameObject();
        InventoryManager.Instance.UpdateInventoryPlayer();
    }

    public void DestroyInventoryTextBoxGameObject()
    {
        if( inventoryTextBoxGameObject != null )
        {
            Destroy(inventoryTextBoxGameObject);
        }
    }

    public void DestroyCurrentlyDraggedItems()
    {
        for (int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count;i++)
        {
            if (inventoryManagementSlot[i].draggedItem != null)
            {
                Destroy(inventoryManagementSlot[i].draggedItem);
            }
        }
    }

    private void PopulatePlayerInventory(InventoryLocation inventoryLocation, List<InventoryItem> playerInventoryList)
    {
        if(inventoryLocation == InventoryLocation.player)
        {
            InitialiseInventoryManagementSlots();
            for(int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.player].Count; i++)
            {
                inventoryManagementSlot[i].itemDetails = InventoryManager.Instance.GetItemDetails(playerInventoryList[i].itemCode);
                inventoryManagementSlot[i].itemQuantity = playerInventoryList[i].itemQuantity;

                if(inventoryManagementSlot[i].itemDetails != null){
                    inventoryManagementSlot[i].InventoryManagementSlotImage.sprite = inventoryManagementSlot[i].itemDetails.itemSprite;
                    inventoryManagementSlot[i].textMeshProGUI.text = inventoryManagementSlot[i].itemQuantity.ToString();
                }
            }
        }
    }

    private void InitialiseInventoryManagementSlots()
    {
        for(int i = 0; i < PlayerController.playerMaximumInventoryCapacity; i++)
        {
            inventoryManagementSlot[i].itemDetails = null;
            inventoryManagementSlot[i].itemQuantity = 0;
            inventoryManagementSlot[i].InventoryManagementSlotImage.sprite = transparent16x16;
           // inventoryManagementSlot[i].textMeshProGUI.text = "";
        }        
    }

}
