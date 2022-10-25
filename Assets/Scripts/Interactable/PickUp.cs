using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : Interactable
{
    [SerializeField] private ItemData mr_Item;
    [SerializeField] private int mi_Amount;
    
    public override void OnInteract()
    {
        base.OnInteract();

        mr_LevelManRef.mg_PlayerRef.GetComponent<Player>().PickUp(mr_Item.ItemIcon);

        StartCoroutine(PickUpInteraction());
    }

    IEnumerator PickUpInteraction()
    {
        float lf_Timer = 0f;

        while (lf_Timer < 1.3f)
        {
            lf_Timer += Time.deltaTime;
            yield return null;
        }

        mr_LevelManRef.FinishedInteracting();

        ItemDatabase.AddToInv(mr_Item, mi_Amount);
        mr_ProgressSystemRef.CompleteQuest(mi_CompleteQuest);
    }

    public override string GetPrompt()
    {
        return "PickUp " + mr_Item.ItemName;
      
    }
}
