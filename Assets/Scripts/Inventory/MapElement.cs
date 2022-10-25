using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapElement : MonoBehaviour
{
    [SerializeField]
    int mi_StartQuest, mi_EndQuest;

    public void DoUpdateElement()
    {
        bool lb_IsActive = false;

        for (int i = mi_StartQuest; i <= mi_EndQuest; i++)
        {
            if (ProgressSystemManager.mr_ProgressSystemManagerInstance.RequestAcceptedQuest(i))
                lb_IsActive = true;
        }

        GetComponent<Image>().enabled = lb_IsActive;
    }
}
