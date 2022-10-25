// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used for the talk interaction.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class Trade : Interactable
{
    [SerializeField] DialogueRunner mc_DialogueRunner;
    private bool mb_HasCompleted = false;
    [SerializeField] string ms_ScriptToRunFirst, ms_ScriptToRunAfter, ms_ScriptToRunAfterNoTrade;
    [SerializeField] string ms_NPCName;
    [SerializeField] TradeData mr_Trade01, mr_Trade02, mr_Trade03;
    [SerializeField] TradeController mc_TradeController;
    [SerializeField] GameObject mg_TalkPos;
    [SerializeField] bool mb_LookDirectionLeft;
    bool mb_D01 = false, mb_D02 = false;
    bool mb_DontDestory = false;

    private void Awake()
    {
        mc_DialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
    }

    public override void OnInteract()
    {
        Debug.Log("TR");

        base.OnInteract();

        mr_LevelManRef.mr_PlayerRef.GoToPosition(mg_TalkPos.transform.position, false);

        StartCoroutine(WaitForPlayerCR());
    }

    IEnumerator WaitForPlayerCR()
    {
        while (mr_LevelManRef.mr_PlayerRef.mb_AutoMoving)
        {
            yield return null;
        }

        mr_LevelManRef.mr_PlayerRef.Face(mb_LookDirectionLeft);

        if (!mb_D01)
        {
            mc_DialogueRunner.StartDialogue(ms_ScriptToRunFirst);
            mc_DialogueRunner.onNodeComplete.AddListener(delegate { DoScriptComplete(); });
        }
        else
        {
            mc_TradeController.SetUp(mr_Trade01, mr_Trade02, mr_Trade03, this);
        }
    }

    public override string GetPrompt()
    {
        return "Press `E` to Trade with " + ms_NPCName;
    }

    void DoScriptComplete()
    {
        if (!mb_D01)
        {
            mc_TradeController.SetUp(mr_Trade01, mr_Trade02, mr_Trade03, this);
            mb_D01 = true;
        }
        else if (!mb_D02)
        {
            mr_ProgressSystemRef.CompleteQuest(mi_CompleteQuest);
            mr_LevelManRef.FinishedInteracting();
            mb_D02 = true;
        }
        else
        {mr_LevelManRef.FinishedInteracting(); }   
    }

    public void ExitTrade()
    {
        if (!mb_D02)
        {
            mc_DialogueRunner.StartDialogue(ms_ScriptToRunAfterNoTrade);
            mc_DialogueRunner.onNodeComplete.AddListener(delegate { DoScriptComplete(); });
            mb_DontDestory = true;
        }
        else
        { mr_LevelManRef.FinishedInteracting(); }
      

    }

    public void EndTrade()
    {
        if (!mb_D02)
        {
            mc_DialogueRunner.StartDialogue(ms_ScriptToRunAfter);
            mb_DontDestory = false;
        }
        else
        { mr_LevelManRef.FinishedInteracting(); }
    }
}