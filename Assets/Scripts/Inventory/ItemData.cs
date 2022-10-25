using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes { Coin, Collectible, Note }

[CreateAssetMenu(menuName = "Nimble/Nimble Item")]
public class ItemData : ScriptableObject
{
    public string ItemID;
    public ItemTypes ItemType;
    public string ItemName;
    [TextArea]
    public string ItemDescription;
    public Sprite ItemIcon;
    public Sprite HistoricalImage;
    public float Value;
    public int AmountCap;
}

public class InventorySlotData
{
    public ItemData Item;
    public int Amount;
    public int Position;

    public InventorySlotData(ItemData a_Item, int a_Amount)
    {
        Item = a_Item;
        Amount = a_Amount;
    }
}