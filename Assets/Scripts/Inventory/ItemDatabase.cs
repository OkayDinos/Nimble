using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class ItemDatabase
{
    static List<InventorySlotData> Inventory = new List<InventorySlotData>();

    static void LoadDataFromSave()
    {
        //* WAITING
        //* Rowan needs to make save system
    }

    static void SaveData()
    {
        //* WAITING
        //* Rowan needs to make save system
    }

    public static void AddToInv(ItemData a_Item, int ai_Amount)
    {

            Inventory.Add(new InventorySlotData(a_Item, 1));

            bool l_NotExist = true;

        switch (a_Item.ItemType)
        {
            case ItemTypes.Coin:
                break;
            case ItemTypes.Collectible:
                ActionLogManager.mr_Instance.AddActionLog($"{a_Item.ItemName} Added To Kist", NotificationType.Custom, a_Item.ItemIcon);
                break;
            case ItemTypes.Note:
                ActionLogManager.mr_Instance.AddActionLog($"Noted In Journal {a_Item.ItemName}", NotificationType.Custom, a_Item.ItemIcon);
                break;
            default:
                break;
        }


            foreach (InventorySlotData item in Inventory)
            {
                if (item.Item == a_Item)
                {
                    item.Amount += ai_Amount;
                    l_NotExist = false;
                }
            }

            if(l_NotExist)
            {
                Inventory.Add(new InventorySlotData(a_Item, ai_Amount));
            }

        ProgressSystemManager.mr_ProgressSystemManagerInstance.SendMessage("InventoryItemUpdate", a_Item);

    }

    public static bool CheckAndUse(ItemData a_Item, int ai_Amount)
    {
        bool lb_Result = false;

            foreach (InventorySlotData item in Inventory.ToArray())
            {
                if (item.Item == a_Item)
                {
                    if(item.Amount >= ai_Amount)
                    {
                        item.Amount -= ai_Amount;
                        lb_Result = true;
                        break;
                    }
                }
            }
        return lb_Result;
    }

    public static void DestroyExistingDatabase()
    {
        Inventory = new List<InventorySlotData>();
    }

    public static bool CheckHas(ItemData a_Item)
    {
        foreach (InventorySlotData item in Inventory.ToArray())
        {
            if (item.Item == a_Item)
            {
                return true;
            }
        }

        return false;
    }

    public static int CheckAmount(ItemData a_Item)
    {
        foreach (InventorySlotData item in Inventory.ToArray())
        {
            if (item.Item == a_Item)
            {
                return item.Amount;
            }
        }

        return 0;
    }
}