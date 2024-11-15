using System;
using System.Collections.Generic;


public static class EventHandler
{
    //Inventory Updated Event
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdatedEvent;
    public static void CallInventoryUpdatedEvent(InventoryLocation inventoryLocation, List<InventoryItem> inventoryList)
    {
        InventoryUpdatedEvent?.Invoke(inventoryLocation, inventoryList);
    }
}
