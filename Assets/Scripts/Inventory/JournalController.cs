using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalController : MonoBehaviour
{
    int mi_Page = 1;
    [SerializeField]
    GameObject[] mg_Pages;
    [SerializeField]
    Button mc_Back, mc_Next;

    public static JournalController mc_JournalControllerInstance;

    private void Awake()
    {
        if (mc_JournalControllerInstance != null)
            Destroy(mc_JournalControllerInstance);
        mc_JournalControllerInstance = this;
    }

    // Update is called once per frame
    void DoUpdate()
    {
        if (mi_Page == 1)
        {
            MapController.mr_Instance.DoUpdateMap();
            mc_Back.interactable = false;
            mc_Next.interactable = true;
        }
        else if (mi_Page == mg_Pages.Length)
        {
            mc_Back.interactable = true;
            mc_Next.interactable = false;
        }
        else
        {
            mc_Back.interactable = true;
            mc_Next.interactable = true;
        }

        foreach (GameObject ag_Page in mg_Pages)
        {
            ag_Page.SetActive(false);
        }
        mg_Pages[mi_Page - 1].SetActive(true);

        foreach (JournalBit ac_Bit in mg_Pages[mi_Page - 1].GetComponentsInChildren<JournalBit>())
        {
            ac_Bit.SendMessage("DoCheck");
        }
    }

    void Back()
    {
        mi_Page--;
        DoUpdate();
    }

    void Next()
    {
        mi_Page++;
        DoUpdate();
    }

    void SetPage(int ai_PageNumber)
    {
        mi_Page = ai_PageNumber;
        DoUpdate();
    }
}
