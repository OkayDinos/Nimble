using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum MenuState { MainMenu, MiniGames, Options, Credits, Welcome }

public enum MenuTextMode { MainGame, MiniGames, None }

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI mc_VersionText;

    [SerializeField]
    GameObject mg_CreditsCanvas, mg_MinigamesCanvas, mg_WelcomeCanvas, mg_MainPanel;

    [SerializeField]
    GameObject mg_OptionsMenu;

    [SerializeField]
    MainMenuCamera mc_Camera;

    [SerializeField]
    GameObject mg_TextStart, mg_TextMini;

    MenuTextMode me_Mode = MenuTextMode.MainGame;

    MenuState me_MenuState = MenuState.MainMenu;

    [SerializeField]
    Button mc_StartButton, mc_MingamesButton;

    // Start is called before the first frame update
    void Start()
    {
        mc_VersionText.text = Application.version;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void SetMenuTextMode(MenuTextMode ae_State)
    {
        me_Mode = ae_State;
        TextUpdate();
    }

    void TextUpdate()
    {
        switch (me_Mode)
        {
            case MenuTextMode.MainGame:
                mg_TextStart.SetActive(true);
                mg_TextMini.SetActive(false);
                break;
            case MenuTextMode.MiniGames:
                mg_TextStart.SetActive(false);
                mg_TextMini.SetActive(true);
                break;
            case MenuTextMode.None:
                mg_TextStart.SetActive(false);
                mg_TextMini.SetActive(false);
                break;
            default:
                break;
        }
    }

    void LoadContext()
    {
        mc_Camera.SendMessage("SetCameraMode", MainMenuCameraMode.Mags);
        mg_WelcomeCanvas.SetActive(true);
        mg_MainPanel.SetActive(false);
        me_MenuState = MenuState.Welcome;

    }
    void LoadOptions()
    {

        mg_OptionsMenu.SetActive(true);
        me_MenuState = MenuState.Options;
    
    }

    void LoadCredits()
    {
        mg_CreditsCanvas.SetActive(true);
        me_MenuState = MenuState.Credits;
    }

    void LoadMinigames()
    {
        SceneManager.LoadSceneAsync("MinigamePlayer");
    }

    void StartMinigame()
    {
        GameManager.SetMinigame(MinigameType.Cooking);
        GameManager.SetRecipe(0);



        StartCoroutine(LoadMinigame());

        IEnumerator LoadMinigame()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MinigameTestScene", LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            this.gameObject.SetActive(false);
        }
    }
    void UnloadOptionMenu()
    {

        mg_OptionsMenu.SetActive(false);
        me_MenuState = MenuState.MainMenu;

    }

    void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void OnBack()
    {
        mg_CreditsCanvas.SetActive(false);
        mg_MinigamesCanvas.SetActive(false);
        me_MenuState = MenuState.MainMenu;
    }

    void OnLMB()
    {
        switch (me_MenuState)
        {
            case MenuState.MainMenu:
                break;
            case MenuState.MiniGames:
                break;
            case MenuState.Options:
            //  mg_OptionsMenu.SetActive(false);
            //  me_MenuState = MenuState.MainMenu;
                break;
            case MenuState.Credits:
                mg_CreditsCanvas.SetActive(false);
                me_MenuState = MenuState.MainMenu;
                break;
            default:
                break;
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
