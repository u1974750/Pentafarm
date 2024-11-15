
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemCode;
    public ItemType itemType;
    public string itemDescription;
    public string itemLongDescription;
    public Sprite itemSprite;
    public GameObject prefab;
    public SnapCell stnapCell;
    public short itemUseGridRadius;
    public bool canBePickedUp;
    public bool canBeDropped;
    public bool canBeSold;
    public int buyPrice;
    public int sellPrice;
}
