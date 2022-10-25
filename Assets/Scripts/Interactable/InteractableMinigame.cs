// This script belongs to Okay Dinos, used for the Nimble project.
// This script inherits from interactable and lets the player start minigames.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableMinigame : Interactable
{
    [SerializeField] MinigameType me_Type;

    [SerializeField] int mi_RecipeID;

    public override void OnInteract()
    {
        base.OnInteract();

        GameManager.SetMinigame(me_Type);
        GameManager.SetRecipe(mi_RecipeID);
        mr_LevelManRef.StartMinigame(me_Type);

        mr_LevelManRef.SetMGFinishesQuestID(mi_CompleteQuest);
    }

    public override string GetPrompt()
    {
        return "Play minigame";
    }
}
