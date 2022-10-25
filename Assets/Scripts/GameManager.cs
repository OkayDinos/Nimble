// This script belongs to Okay Dinos, used for the Nimble project.
// This script is used for managing different data in the game.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayerNormal, PlayerInteract, Paused, InventoryOpen}

public enum ControlScheme { Keyboard, Gamepad }

public static class GameManager
{
    static MinigameType me_CurrentMinigame;
    static int mi_CurrentRecipeID;
    static GameState me_CurrentGameState = GameState.PlayerNormal;
    static ControlScheme me_CurrentControlScheme = ControlScheme.Keyboard;

    static void Init()
    {

    }

    static void Pause()
    {

    }

    public static void SetControlScheme(ControlScheme ae_ControlScheme)
    {
        me_CurrentControlScheme = ae_ControlScheme;
    }

    public static ControlScheme GetControlScheme()
    {
        return me_CurrentControlScheme;
    }

    public static void SetCurrentGameState(GameState ae_NewGameState)
    {
        me_CurrentGameState = ae_NewGameState;
    }

    public static GameState GetCurrentGameState()
    {
        return me_CurrentGameState;
    }

    public static void SetMinigame(MinigameType ae_newMGType)
    {
        me_CurrentMinigame = ae_newMGType;
    }

    public static MinigameType GetMinigame()
    {
        return me_CurrentMinigame;
    }

    public static void SetRecipe(int ai_RecipeID)
    {
        mi_CurrentRecipeID = ai_RecipeID;
    }

    public static int GetRecipeID()
    {
        return mi_CurrentRecipeID;
    }
}
