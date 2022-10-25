using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigamePlayer : MonoBehaviour
{
    public static List<MinigameManager> mr_MinigameMans = new List<MinigameManager>();

    [SerializeField] List<GameObject> mg_MenuUI = new List<GameObject>();

    InteractionPromptManager mr_IntProManRef;

    private void Start()
    {
        mr_IntProManRef = InteractionPromptManager.mr_Instance;
    }

    public void ButtonMainMenu()
    {
        GameManager.SetCurrentGameState(GameState.PlayerNormal);

        SceneManager.LoadScene(0);
    }

    public void ButtonPlayCookingMinigame(int ai_RecipeID)
    {
        GameManager.SetRecipe(ai_RecipeID);
        StartMinigame(MinigameType.Cooking);
    }

    public void ButtonPlayGuttingMinigame(int ai_RecipeID)
    {
        GameManager.SetRecipe(ai_RecipeID);
        StartMinigame(MinigameType.FishPrep);
    }

    public void ButtonPlayKnittingMinigame(int ai_RecipeID)
    {
        GameManager.SetRecipe(ai_RecipeID);
        StartMinigame(MinigameType.Knitting);
    }

    public void StartMinigame(MinigameType ae_Type)
    {
        foreach(GameObject lg_UIE in mg_MenuUI)
        {
            lg_UIE.SetActive(false);
        }    

        GameManager.SetMinigame(ae_Type);
        StartCoroutine(LoadMinigame());
    }

    IEnumerator LoadMinigame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MinigameTestScene", LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        StartCoroutine(MinigameWait());
    }

    IEnumerator MinigameWait()
    {
        while (mr_MinigameMans[0].mb_Leave == false)
        {
            yield return null;
        }

        EndMinigame();
    }

    public void EndMinigame()
    {
        StartCoroutine(UnloadMinigame());
    }

    IEnumerator UnloadMinigame()
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync("MinigameTestScene");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        mr_IntProManRef.ForceClear();

        foreach (GameObject lg_UIE in mg_MenuUI)
        {
            lg_UIE.SetActive(true);
        }
    }
}
