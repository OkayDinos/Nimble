using UnityEngine;

public class NPCLocationController : MonoBehaviour
{
    public static NPCLocationController mc_NPCLocationControllerInstance;
    
    [SerializeField]
    NPCLocationQuestUpdate[] mc_QuestUpdates;

    private void Awake()
    {
        if (mc_NPCLocationControllerInstance != null)
            Destroy(mc_NPCLocationControllerInstance);
        mc_NPCLocationControllerInstance = this;
    }

    void UpdateLocationsFromQuest(int ai_QuestID)
    {
        foreach (NPCLocationQuestUpdate ac_QuestUpdate in mc_QuestUpdates)
        {
            Debug.Log("QuestCheck: " + ai_QuestID);
            if(ac_QuestUpdate.GetQuest() == ai_QuestID)
            {
                Debug.Log("QuestCheckMatch");
                ac_QuestUpdate.GetNPC().transform.position = ac_QuestUpdate.GetTarget().position;
                foreach (SpriteRenderer ac_SpriteRenderer in ac_QuestUpdate.GetNPC().GetComponentsInChildren<SpriteRenderer>())
                {
                    ac_SpriteRenderer.sortingLayerName = ac_QuestUpdate.GetSortingLayer();
                }

            }
        }
    }
}