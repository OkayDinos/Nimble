// This script belongs to Okay Dinos, used for the Nimble project.
// This is a base script for interactables.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Apr 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected LevelManager mr_LevelManRef;

    protected ProgressSystemManager mr_ProgressSystemRef;

    protected ActionLogManager mr_ActLogManRef;

    [SerializeField] LocationLayer me_IsOnLayer;

    [SerializeField] protected int mi_CompleteQuest;

    [SerializeField] protected int mi_QuestRequired;

    [SerializeField] protected int mi_LockedAfterQuest;

    [SerializeField] protected OutlineController mc_OutlineObject;

    public bool mb_IsUnlocked;

    public void Start()
    {
        mr_LevelManRef = LevelManager.mr_Instance;
        mr_ProgressSystemRef = ProgressSystemManager.mr_ProgressSystemManagerInstance;
        mr_ActLogManRef = ActionLogManager.mr_Instance;

        mb_IsUnlocked = true;

        if ((mi_QuestRequired >= 1 && !mr_ProgressSystemRef.RequestCompleteQuest(mi_QuestRequired)) || 
            (mi_LockedAfterQuest >= 1 && mr_ProgressSystemRef.RequestCompleteQuest(mi_LockedAfterQuest)))
        {
            mb_IsUnlocked = false;
        } 
    }

    public OutlineController GetOutlineObject()
    {
        return mc_OutlineObject;
    }

    public void OnEnable()
    {
        LevelManager.mg_Interactables.Add(this);
    }

    public void OnDisable()
    {
        LevelManager.mg_Interactables.Remove(this);
    }

    public virtual int TryWalkInt()
    {
        return 0;
    }

    public virtual void TryUnlock(int ai_QuestID)
    {
        if (mi_QuestRequired == ai_QuestID)
        {
            mb_IsUnlocked = true;
        }
        else if (mi_LockedAfterQuest == ai_QuestID)
        {
            mb_IsUnlocked = false;
        }
    }

    public bool GetUnlockStatus()
    {
        return mb_IsUnlocked;
    }

    public virtual void OnInteract()
    {
    }

    public virtual string GetPrompt()
    {
        return "Press 'E' to interact";
    }

    public LocationLayer GetLayer()
    {
        return me_IsOnLayer;
    }

    public void SetLayer(LocationLayer ae_SetLayer)
    {
        me_IsOnLayer = ae_SetLayer;
    }

    // Changes if interactable is on interactable layer or not - CP
    public void Hide(bool ab_IsHide)
    {
        bool lb_FriendIsUnlocked = false;

        foreach (Interactable ac_Interactable in this.gameObject.GetComponents<Interactable>())
        {
            if(ac_Interactable.GetUnlockStatus())
            {
                lb_FriendIsUnlocked = true;
            }
        }

        if (ab_IsHide || !lb_FriendIsUnlocked)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = 12;
        }
    }
}
