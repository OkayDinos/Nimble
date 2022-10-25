// This script belongs to Okay Dinos, used for the Nimble project.
// This script inherits from interactable and lets the player open their kist.
// Author(s): Morgan Finney.
// Date created - last edited: Apr 2022 - Apr 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenKist : Interactable
{
 
    [SerializeField] InventoryController mc_InventoryController;


    public override void OnInteract()
    {
        base.OnInteract();

        InventoryController.mc_Instance.SendMessage("SetKist");
        mr_LevelManRef.FinishedInteracting();
    }

    public override string GetPrompt()
    {
        return "Open Kist";
    }
}
