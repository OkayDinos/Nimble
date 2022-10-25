// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used for the talk interaction.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class Proxim : Interactable
{
    [SerializeField] DialogueRunner mc_DialogueRunner;
    [SerializeField] string[] ms_ScriptToRunName;
    [SerializeField] GameObject mg_TalkPos;
    [SerializeField] bool mb_LookDirectionLeft;
    [SerializeField] LayerMask mr_InteractableLayer;

    private void Awake()
    {
        mc_DialogueRunner = GameObject.FindObjectOfType<DialogueRunner>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && this.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Debug.Log("Player");

            if(ProgressSystemManager.mr_ProgressSystemManagerInstance.RequestAcceptedQuest(mi_CompleteQuest))
            {
                LevelManager.mr_Instance.mr_PlayerRef.ProximInteract();
                Debug.Log("Quest" + mi_CompleteQuest);
                LevelManager.mr_Instance.mr_PlayerRef.GoToPosition(mg_TalkPos.transform.position, false);

                StartCoroutine(WaitForPlayerCR());
            }
        }
    }

    IEnumerator WaitForPlayerCR()
    {
        while (LevelManager.mr_Instance.mr_PlayerRef.mb_AutoMoving)
        {
            yield return null;
        }

        LevelManager.mr_Instance.mr_PlayerRef.Face(mb_LookDirectionLeft);

        int li_Random = Random.Range(0, ms_ScriptToRunName.Length - 1);

        mc_DialogueRunner.StartDialogue(ms_ScriptToRunName[li_Random]);
        mc_DialogueRunner.onNodeComplete.AddListener(delegate { DoScriptComplete(); });
    }

    void DoScriptComplete()
    {
        ProgressSystemManager.mr_ProgressSystemManagerInstance.CompleteQuest(mi_CompleteQuest);
        LevelManager.mr_Instance.FinishedInteracting();
    }
}