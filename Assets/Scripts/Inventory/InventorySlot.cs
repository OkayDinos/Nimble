using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI mc_SlotNumber, mc_Question;
    [SerializeField]
    GameObject mc_ItemIcon;

    ItemData mr_Item;

    [SerializeField]
    Material normal, gry;

    [SerializeField]
    int mi_SlotID;

    public void SetIcon(Sprite a_Icon)
    {
        mc_ItemIcon.GetComponent<Image>().sprite = a_Icon;
    }

    public void SetItem(ItemData ar_Item)
    {
        mr_Item = ar_Item;
    }

    public void SetAmount(int a_Amount)
    {
        mc_SlotNumber.text = a_Amount.ToString();

        if (a_Amount <= 0)
        {
            mc_Question.gameObject.SetActive(true);
            mc_ItemIcon.GetComponent<Image>().material = gry;
        }
        else
        {
            mc_Question.gameObject.SetActive(false);
            mc_ItemIcon.GetComponent<Image>().material = normal;
        }
    }

    public int GetID()
    {
        return mi_SlotID;
    }

    public ItemData GetItemData()
    {
        return mr_Item;
    }
}
