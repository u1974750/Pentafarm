using UnityEngine;

public class Comodity : MonoBehaviour
{
    public GameObject itemDrop;
    
    public void Demolition()
    {
        Item item = itemDrop.GetComponent<Item>();
        if(item != null)
        {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
            if (itemDetails.canBeDropped)
            {
                GameObject i = Instantiate(itemDrop, transform.position,Quaternion.identity);
                i.transform.position += new Vector3(0f, 0.25f, 0f);
            }
        }      

        Destroy(transform.gameObject);
    }
    
}
