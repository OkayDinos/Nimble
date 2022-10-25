using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressSystemManager : MonoBehaviour
{
    public static ProgressSystemManager mr_ProgressSystemManagerInstance;

    public List<OkayDinos.Nimble.ProgressSystem.QuestData> questList = new List<OkayDinos.Nimble.ProgressSystem.QuestData>(); //master quest list
    public List<OkayDinos.Nimble.ProgressSystem.QuestData> currentQuestList = new List<OkayDinos.Nimble.ProgressSystem.QuestData>();

    [SerializeField]
    GameObject mg_GO;

    LevelManager mr_LevelManRef;


    private void Start()
    {
        mr_ProgressSystemManagerInstance.AcceptQuest(1);

        mr_LevelManRef = LevelManager.mr_Instance;
    }

    private void Awake()
    {
        if (mr_ProgressSystemManagerInstance == null)
        {
            mr_ProgressSystemManagerInstance = this;
        }
        else if (mr_ProgressSystemManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void InventoryItemUpdate(ItemData ar_ItemData = null)
    {
        if(currentQuestList[0].QuestType == OkayDinos.Nimble.ProgressSystem.QuestType.Item)
        {
            if (currentQuestList[0].ItemData == ar_ItemData || ar_ItemData == null)
            {
                if (ItemDatabase.CheckHas(currentQuestList[0].ItemData))
                {
                    if (ItemDatabase.CheckAmount(currentQuestList[0].ItemData) >= currentQuestList[0].ItemAmount)
                    {
                        CompleteQuest(currentQuestList[0].mi_id);
                    }
                }
            }
        }
    }
  
    //ACCEPT QUEST
    public void AcceptQuest(int questID)
    {

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].mi_id == questID && questList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.NOT_COMPLETE)
            {
                currentQuestList.Add(questList[i]);
                questList[i].mc_Progress = OkayDinos.Nimble.ProgressSystem.ProgressState.CURRENT;

                GameObject.FindObjectOfType<HUDController>().SendMessage("SetQuestDescription", questList[i].ms_questObjective);
            }
        }

    }


    //COMPLETE QUEST
    public void CompleteQuest(int questID)
    {
        if(questID == 13)
        {
            mg_GO.SetActive(true);
        }

        for (int i = 0; i < currentQuestList.Count; i++)
        {
            if (currentQuestList[i].mi_id == questID && currentQuestList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.CURRENT)
            {
                currentQuestList[i].mc_Progress = OkayDinos.Nimble.ProgressSystem.ProgressState.COMPLETE;
                currentQuestList.Remove(currentQuestList[i]);

                mr_LevelManRef.QuestFinished(questID);

                NPCLocationController.mc_NPCLocationControllerInstance.SendMessage("UpdateLocationsFromQuest", questID);

                ActionLogManager.mr_Instance.AddActionLog($"Quest Complete", NotificationType.Blue, af_TimeShown: 4f);

            }
            
        }
        // check for chain quests
        CheckChainQuest(questID);
    }


    //CHEC CHAIN QUEST
    void CheckChainQuest(int questID) 
    {
        int tempID = 0;

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].mi_id == questID && questList[i].mi_nextQuest > 0)
            {
                tempID = questList[i].mi_nextQuest;
            }
        }

        if (tempID > 0)
        {
            for (int i = 0; i < questList.Count; i++)
            {
                if (questList[i].mi_id == tempID &&  questList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.NOT_COMPLETE)
                {
                    questList[i].mc_Progress = OkayDinos.Nimble.ProgressSystem.ProgressState.CURRENT;
                    currentQuestList.Add(questList[i]);
                    GameObject.FindObjectOfType<HUDController>().SendMessage("SetQuestDescription", questList[i].ms_questObjective);
                    ActionLogManager.mr_Instance.AddActionLog($"New Objective! Press Q", NotificationType.Yellow, af_TimeShown: 6f);
                }
            }
        }

        InventoryItemUpdate();
    }

  

    // BOOLS
    public bool RequestAvailableQuest(int questId)
    {

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].mi_id == questId && questList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.NOT_COMPLETE)
            {
                return true;
            }
        }

        return false;

    }
    public bool RequestAcceptedQuest(int questId)
    {

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].mi_id == questId && questList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.CURRENT)
            {
                return true;
            }
        }

        return false;

    }
    public bool RequestCompleteQuest(int questId)
    {

        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].mi_id == questId && questList[i].mc_Progress == OkayDinos.Nimble.ProgressSystem.ProgressState.COMPLETE)
            {
                return true;
            }
        }

        return false;

    }

   

}
