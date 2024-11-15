using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryBar : MonoBehaviour
{
    private const int MIN_POS = 14;
    private const int MAX_POS = 245;
    [SerializeField] private Sprite blankSprite = null;
    [SerializeField] private UIInventorySlot[] inventorySlot = null;
    private RectTransform selectedPos = null;
    private float actualPos = 14;
    private int activePosArray = 0;

    [HideInInspector] public GameObject inventoryTextBoxGameobject;
    public GameObject selectedObject;
    public int ActivePositionArray { get { Debug.Log("GETTER"); return activePosArray; } }


    // Start is called before the first frame update
    private void Awake()
    {
        selectedPos = selectedObject.GetComponent<RectTransform>();
        selectedPos.anchoredPosition = new Vector3(MIN_POS, 0, 0);
        actualPos = MIN_POS;
    }

    private void OnDisable()
    {
        EventHandler.InventoryUpdatedEvent -= InventoryUpdated;
        DestroyInventoryTextBoxGameObject();
    }

    private void OnEnable()
    {
        EventHandler.InventoryUpdatedEvent += InventoryUpdated;
        InventoryManager.Instance.UpdateInventoryPlayer();
    }
    
    private void Update()
    {
        updateActualItem();
    }

    public void DestroyInventoryTextBoxGameObject()
    {
        if (inventoryTextBoxGameobject != null)
        {
            Destroy(inventoryTextBoxGameobject);
        }
    }

    private void ClearInventorySlots()
    {
        if(inventorySlot.Length > 0)
        {
            for(int i = 0; i < inventorySlot.Length; i++)
            {
                inventorySlot[i].InventorySlotImage.sprite = blankSprite;
                inventorySlot[i].textMeshProGUI.text = "";
                inventorySlot[i].itemDetails = null;
                inventorySlot[i].itemQuantity = 0;
            }
        }
    }

    private void InventoryUpdated(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        if(inventoryLocation == InventoryLocation.player)
        {
            ClearInventorySlots();

            if(inventorySlot.Length > 0 && inventoryList.Count > 0)
            {
                for(int i = 0; i < inventorySlot.Length; i++)
                {
                    if(i < inventoryList.Count)
                    {
                        int itemCode = inventoryList[i].itemCode;
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(itemCode);

                        if(itemDetails != null)
                        {
                            inventorySlot[i].InventorySlotImage.sprite = itemDetails.itemSprite;
                            inventorySlot[i].itemDetails = itemDetails;
                            inventorySlot[i].itemQuantity = inventoryList[i].itemQuantity;
                            if(inventoryList[i].itemQuantity == 0)
                                inventorySlot[i].textMeshProGUI.text = "";
                            else
                                inventorySlot[i].textMeshProGUI.text = inventoryList[i].itemQuantity.ToString();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    private void updateActualItem()
    {
        float inputValue = Input.mouseScrollDelta.y;
        //detecta input de la ruedecita del raton
        if (inputValue > 0f && actualPos != MIN_POS) activePosArray -= 1; 
        else if (inputValue < 0f && actualPos != MAX_POS)  activePosArray += 1;
        
        //detecta inputs linea numerica teclado
        
        if(Input.GetKeyDown(KeyCode.Alpha1)) activePosArray = 0;
        if(Input.GetKeyDown(KeyCode.Alpha2)) activePosArray = 1;
        if(Input.GetKeyDown(KeyCode.Alpha3)) activePosArray = 2;
        if(Input.GetKeyDown(KeyCode.Alpha4)) activePosArray = 3;
        if(Input.GetKeyDown(KeyCode.Alpha5)) activePosArray = 4;
        if(Input.GetKeyDown(KeyCode.Alpha6)) activePosArray = 5;
        if(Input.GetKeyDown(KeyCode.Alpha7)) activePosArray = 6;
        if(Input.GetKeyDown(KeyCode.Alpha8)) activePosArray = 7;
        if(Input.GetKeyDown(KeyCode.Alpha9)) activePosArray = 8;
        if(Input.GetKeyDown(KeyCode.Alpha0)) activePosArray = 9;
        if(Input.GetKeyDown(KeyCode.F9)) activePosArray = 10;
        if(Input.GetKeyDown(KeyCode.F10)) activePosArray = 11;
        

        InventoryManager.Instance.SelectedItem = activePosArray;
        actualPos = (activePosArray * 21) + 14;
        selectedPos.anchoredPosition = new Vector3(actualPos, 0, 0);
    }


}
