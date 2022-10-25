// This script belongs to Okay Dinos, used for the Nimble project.
// This script spawns and manages how the minigame instance.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager mr_Instance;

    protected MinigameType me_gameType;

    protected MinigameBase mr_minigame;

    public bool mb_Leave;

    public bool mb_MinigameCompleted;

    [SerializeField]
    GameObject mg_minigameCookingPrefab;

    [SerializeField]
    GameObject mg_minigameKnittingPrefab;

    [SerializeField]
    GameObject mg_minigameFishPrepPrefab;

    [SerializeField]
    GameObject ButtonStart;

    [SerializeField]
    GameObject Complete;

    [SerializeField]
    GameObject SpeedRating;

    [SerializeField]
    List<GameObject> mg_Stars = new List<GameObject>();

    [SerializeField]
    GameObject ExitButton;

    bool mb_StartActive;

    private void Awake()
    {
        if (mr_Instance != null)
            Destroy(mr_Instance);
        mr_Instance = this;

        NewGame(GameManager.GetMinigame());
        mb_Leave = false;
        mb_MinigameCompleted = false;

        mb_StartActive = true;
    }

    private void OnEnable()
    {
        LevelManager.mr_MinigameMans.Add(this);
        MinigamePlayer.mr_MinigameMans.Add(this);
    }

    private void OnDisable()
    {
        LevelManager.mr_MinigameMans.Remove(this);
        MinigamePlayer.mr_MinigameMans.Remove(this);
    }

    public void OnStartPressed()
    {
        ButtonStart.SetActive(false);

        mb_StartActive = false;

        mr_minigame.Begin();
    }

    public void OnLeavePressed()
    {
        mb_Leave = true;
    }

    public void MinigameComplete(float af_TimeScore)
    {
        Complete.SetActive(true);
        mb_MinigameCompleted = true;

        foreach (GameObject lg_star in mg_Stars)
        {
            lg_star.SetActive(false);
        }

        int li_StarsWon = 0;

        if (af_TimeScore > 0.8f)
        {
            SpeedRating.GetComponent<Text>().text = "Speed Rating: Seagull Speed!";
            li_StarsWon = 3;
        }
        else if (af_TimeScore > 0.4f)
        {
            SpeedRating.GetComponent<Text>().text = "Speed Rating: Boat Speed!";
            li_StarsWon = 2;
        }
        else if (af_TimeScore > -0.2f)
        {
            SpeedRating.GetComponent<Text>().text = "Speed Rating: Mum Speed!";
            li_StarsWon = 1;            
        }
        else
        {
            SpeedRating.GetComponent<Text>().text = "Speed Rating: Gran Speed!";
            li_StarsWon = 0;
        }

        for (int i = 0; i < li_StarsWon; i++)
        {
            StartCoroutine(ExpandAnim(mg_Stars[i]));
        }
    }

    IEnumerator ExpandAnim(GameObject ag_Star)
    {
        float lf_Timer = 0f;
        float lf_Time = 0.8f;

        float lf_Size = ag_Star.transform.localScale.x;
        float lf_AmountRotated = 0f;
        ag_Star.SetActive(true);
        
        while (lf_Timer < lf_Time)
        {
            lf_Timer += Time.deltaTime;

            ag_Star.transform.localScale = new Vector3((lf_Size * (lf_Timer / lf_Time)), (lf_Size * (lf_Timer / lf_Time)), 1);

            ag_Star.transform.Rotate(0f, 0f, 360f * 1 / 0.3f * Time.deltaTime, Space.Self);

            lf_AmountRotated += 360f * 1 / 0.3f * Time.deltaTime;

            yield return null;
        }
        ag_Star.transform.Rotate(0f, 0f, 360 - lf_AmountRotated, Space.Self);

        ag_Star.transform.localScale = new Vector3(lf_Size, lf_Size, 1);
    }

    // This method initialises a new minigame based on minigame type (also stores ref) - CP
    void NewGame(MinigameType ae_gameType)
    {
        me_gameType = ae_gameType;

        GameObject lg_minigame = null;

        switch (me_gameType)
        {
            case MinigameType.Cooking:
                if (mg_minigameCookingPrefab)
                    lg_minigame = Instantiate(mg_minigameCookingPrefab);
                mr_minigame = lg_minigame.GetComponent<MinigameCooking>();
                break;
            case MinigameType.Knitting:
                if (mg_minigameKnittingPrefab)
                    lg_minigame = Instantiate(mg_minigameKnittingPrefab);
                mr_minigame = lg_minigame.GetComponent<MinigameKnitting>();
                break;
            case MinigameType.FishPrep:
                if (mg_minigameFishPrepPrefab)
                    lg_minigame = Instantiate(mg_minigameFishPrepPrefab);
                mr_minigame = lg_minigame.GetComponent<MinigameCooking>();
                break;
            default:
                return;
        }

        lg_minigame.transform.parent = transform;

        if (mr_minigame != null)
        {
            ButtonStart.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (GameManager.GetCurrentGameState() == GameState.Paused)
        {
            ButtonStart.SetActive(false);
            ExitButton.SetActive(false);
        }
        else
        {
            if (mb_StartActive)
                ButtonStart.SetActive(true);
            
            ExitButton.SetActive(true);
        }
    }
}