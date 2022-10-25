using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum NotificationType { Yellow, Blue, Custom }

public class ActionLogManager : MonoBehaviour
{
    public static ActionLogManager mr_Instance;

    public List<GameObject> mg_ActionLogsList = new List<GameObject>();

    [SerializeField] GameObject mg_ActionLog;

    [SerializeField]
    Sprite mr_Yellow, mr_Blue;

    private void Awake()
    {
        if (mr_Instance != null)
            Destroy(mr_Instance);
        mr_Instance = this;
    }

    public GameObject AddActionLog(string as_Text, NotificationType ae_NotificationType, Sprite ar_Sprite = null, float af_TimeShown = 1.5f)
    {
        GameObject lg_Object = Instantiate(mg_ActionLog);
        lg_Object.transform.SetParent(transform, false);
        lg_Object.GetComponentInChildren<TextMeshProUGUI>().text = as_Text;

        switch (ae_NotificationType)
        {
            case NotificationType.Yellow:
                lg_Object.GetComponentInChildren<Image>().sprite = mr_Yellow;
                break;
            case NotificationType.Blue:
                lg_Object.GetComponentInChildren<Image>().sprite = mr_Blue;
                break;
            case NotificationType.Custom:
                if (ar_Sprite)
                    lg_Object.GetComponentInChildren<Image>().sprite = ar_Sprite;
                else
                    lg_Object.GetComponentInChildren<Image>().enabled = false;
                break;
            default:
                break;
        }

        if (af_TimeShown > 0)
        {
            mg_ActionLogsList.Add(lg_Object);
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

            // Fade
            mg_Ref.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1 - (lf_Timer / af_Seconds));
            mg_Ref.GetComponentInChildren<TextMeshProUGUI>().alpha = 1 - (lf_Timer / af_Seconds);

            await Task.Yield();
        }

        mg_ActionLogsList.Remove(mg_Ref);
        Destroy(mg_Ref);
    }

    private void Update()
    {
        int li_i = 0;
        foreach (GameObject lg_Prompts in mg_ActionLogsList)
        {
            lg_Prompts.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 105f * (float)li_i, 0);

            li_i++;
        }
    }
}
