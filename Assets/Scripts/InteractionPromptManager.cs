using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InteractionControl { E = 0, W, S, RightTrigger, GamepadSouth, Space, F, I, LMB, P, Q, GamepadWest, JoyStick }

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager mr_Instance;

    public List<GameObject> mg_InteractionPrompts = new List<GameObject>();
    
    [SerializeField] List<Sprite> mc_ControlSprites = new List<Sprite>();

    [SerializeField] GameObject mg_InteractionPromptCell;

    private void Awake()
    {
        if (mr_Instance != null)
            Destroy(mr_Instance);
        mr_Instance = this;
    }

    public void ClearAllPrompts()
    {
        while (mg_InteractionPrompts.Count > 0)
        {
            GameObject lg_Ref = mg_InteractionPrompts[0];
            mg_InteractionPrompts.Remove(lg_Ref);
            Destroy(lg_Ref);
        }
    }

    public GameObject AddInteractionPrompt(string as_Text, InteractionControl ae_Control, float af_TimeShown = 0f)
    {
        GameObject lg_Object = Instantiate(mg_InteractionPromptCell);
        lg_Object.transform.SetParent(transform, false);
        lg_Object.GetComponentInChildren<Image>().sprite = mc_ControlSprites[(int)ae_Control];
        lg_Object.GetComponentInChildren<TextMeshProUGUI>().text = as_Text;

        if (af_TimeShown > 0)
        {
            mg_InteractionPrompts.Add(lg_Object);
            RemoveAfter(lg_Object, af_TimeShown);
        }

        return lg_Object;
    }

    async void RemoveAfter(GameObject mg_Ref, float af_Seconds)
    {
        float lf_Timer = 0f;

        while (lf_Timer < af_Seconds)
        {
            lf_Timer += Time.deltaTime;
            await Task.Yield();
        }

        mg_InteractionPrompts.Remove(mg_Ref);
        Destroy(mg_Ref);
    }

    public void ForceClear()
    {
        while (mg_InteractionPrompts.Count > 0)
        {
            GameObject mg_Ref = mg_InteractionPrompts[0];
            mg_InteractionPrompts.RemoveAt(0);
            Destroy(mg_Ref);
        }
    }

    private void Update()
    {
        int li_i = 0;
        foreach (GameObject lg_Prompts in mg_InteractionPrompts)
        {
            lg_Prompts.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 105f * (float)li_i, 0);

            li_i++;
        }
    }
}
