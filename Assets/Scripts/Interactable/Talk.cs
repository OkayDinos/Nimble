// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used for the talk interaction.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class Talk : Interactable
{
    [SerializeField] DialogueRunner mc_DialogueRunner;
    private bool mb_HasCompleted = false;
    [SerializeField] string[] ms_ScriptToRunName;
    [SerializeField] string ms_NPCName;
    [SerializeField] GameObject mg_TalkPos;
    [SerializeField] bool mb_LookDirectionLeft;

    [SerializeField] NPC NPCRef;

    private void Awake()
    {
        mc_DialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
        NPCRef.me_CurrentState = NPCStates.Waving;
    }

    public override void TryUnlock(int ai_QuestID)
    {
        if (mi_QuestRequired == ai_QuestID)
        {
            mb_IsUnlocked = true;
            NPCRef.me_CurrentState = NPCStates.Waving;
        }
        else if (mi_LockedAfterQuest == ai_QuestID)
        {
            mb_IsUnlocked = false;
        }
    }

    public override void OnInteract()
    {
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

        NPCRef.me_CurrentState = NPCStates.Idle;

        int li_Random = Random.Range(0, ms_ScriptToRunName.Length - 1);

        Debug.Log("Starting Diol" + ms_NPCName);

        mc_DialogueRunner.StartDialogue(ms_ScriptToRunName[li_Random]);

        mc_DialogueRunner.onNodeComplete.AddListener(delegate { DoScriptComplete(); });
    }

    public override string GetPrompt()
    {
        return "Talk with " + ms_NPCName;
    }

    void DoScriptComplete()
    {
        Debug.Log("Finish talking " + ms_NPCName);
        mr_ProgressSystemRef.CompleteQuest(mi_CompleteQuest);
        mr_LevelManRef.FinishedInteracting();
        mc_DialogueRunner.onNodeComplete.RemoveAllListeners();
    }
}