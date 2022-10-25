// This script belongs to Okay Dinos, used for the Nimble Project.
// This script controlls Pause functions.
// Author(s): Morgan Finney.
// Date created: Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PauseState {  Null, Paused, Options }


public class PauseController : MonoBehaviour
{
    [SerializeField]
    GameObject mo_PausePanel, mo_OptionsPanel;
    PauseState me_PauseState = PauseState.Null;

    private void Start()
    {
        me_PauseState = PauseState.Null;
    }

    void OnPause()
    {
        if (GameManager.GetCurrentGameState() == GameState.PlayerNormal || GameManager.GetCurrentGameState() == GameState.Paused)
        {
            switch (me_PauseState)
            {
                case PauseState.Null:
                    mo_PausePanel.SetActive(true);
                    me_PauseState = PauseState.Paused;
                    GameManager.SetCurrentGameState(GameState.Paused);
                    break;
                case PauseState.Paused:
                    mo_PausePanel.SetActive(false);
                    me_PauseState = PauseState.Null;
                    GameManager.SetCurrentGameState(GameState.PlayerNormal);
                    break;
                case PauseState.Options:
                    mo_PausePanel.SetActive(true);
                    mo_OptionsPanel.SetActive(false);
                    me_PauseState = PauseState.Paused;
                    break;
                default:
                    break;
            }
        }
    }

    void EndPause()
    {
        mo_PausePanel.SetActive(false);
        me_PauseState = PauseState.Null;
        GameManager.SetCurrentGameState(GameState.PlayerNormal);
    }

    void ShowOptions()
    {
        mo_PausePanel.SetActive(false);
        mo_OptionsPanel.SetActive(true);
        me_PauseState = PauseState.Options;
    }

    void ExitToMenu()
    {
        ProgressSystemManager lr_ProgressSystem = ProgressSystemManager.mr_ProgressSystemManagerInstance;
        GameObject lg_ProgressSystem = lr_ProgressSystem.gameObject;
        GameObject.Destroy(lg_ProgressSystem);
        GameManager.SetCurrentGameState(GameState.PlayerNormal);

        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
    }
}
