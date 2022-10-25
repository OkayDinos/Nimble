using UnityEngine;

public class NPCLocationQuestUpdate : MonoBehaviour
{
    [SerializeField]
    Transform mc_NewLocation;

    [SerializeField]
    int mi_MoveAfterQuest;

    [SerializeField]
    GameObject mg_NPC;

    [SerializeField]
    string ms_NewSortingLayer;

    public string GetSortingLayer()
    {
        return ms_NewSortingLayer;
    }

    public int GetQuest()
    {
        return mi_MoveAfterQuest;
    }

    public Transform GetTarget()
    {
        return mc_NewLocation;
    }

    public GameObject GetNPC()
    {
        return mg_NPC;
    }
}