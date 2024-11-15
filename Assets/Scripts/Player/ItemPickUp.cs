
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    AudioPlayerManagment audioPlayer;
    private void Start()
    {
        audioPlayer = GetComponent<AudioPlayerManagment>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Item"))
        {
          
            Item item = collision.GetComponent<Item>();

            if(item != null)
            {
                
                //Get item details
                ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.ItemCode);
                
                if (itemDetails.canBePickedUp)
                {
                    audioPlayer.playTakeItem();
                    InventoryManager.Instance.AddItem(InventoryLocation.player, item.ItemCode);
                    Destroy(collision.gameObject);
                }
            }     
        }        
    }
}
