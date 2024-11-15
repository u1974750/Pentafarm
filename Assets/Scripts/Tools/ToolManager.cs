using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{

    public GameObject Hoeing;
    public GameObject WateringCan;
    public GameObject BrakingTool;
    public GameObject Basket;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InventoryManager.Instance.GetItemActive()!= null)
        {
            switch (InventoryManager.Instance.GetItemActive().itemType)
            {
                case ItemType.Hoeing_tool:
                    Hoeing.SetActive(true);
                    WateringCan.SetActive(false);
                    BrakingTool.SetActive(false);
                    Basket.SetActive(false);
                    break;
                case ItemType.Watering_tool:
                    WateringCan.SetActive(true);
                    Hoeing.SetActive(false);
                    BrakingTool.SetActive(false);
                    Basket.SetActive(false);
                    break;
                case ItemType.Breaking_tool:
                    BrakingTool.SetActive(true);
                    Hoeing.SetActive(false);
                    WateringCan.SetActive(false);
                    Basket.SetActive(false);
                    break;
                case ItemType.Commodity:
                    BrakingTool.SetActive(true);
                    Hoeing.SetActive(false);
                    WateringCan.SetActive(false);
                    Basket.SetActive(false);
                    break;
                case ItemType.Collecting_tool:
                    Basket.SetActive(true);
                    Hoeing.SetActive(false);
                    WateringCan.SetActive(false);
                    BrakingTool.SetActive(false);
                    break;
                default:
                    Hoeing.SetActive(false);
                    WateringCan.SetActive(false);
                    BrakingTool.SetActive(false);
                    Basket.SetActive(false);
                    break;
            }
       

        }
    }
}
