// This script belongs to Okay Dinos, used for the Nimble Project.
// This script controlls HUD elemants.
// Author(s): Morgan Finney.
// Date created: Mar 2022.

using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField]
    RectTransform mc_MainRectTransform, mc_ArrowRectTransform;

    [SerializeField]
    float mf_Time;
    float mf_MainCurrentPositionY = 360;
    float mf_ArrowCurrentRotationX = 180;

    [SerializeField]
    TextMeshProUGUI mc_DescriptionText;

    private void Start()
    {
        OnDropQuest();
    }

    private void Update()
    {
        mc_MainRectTransform.anchoredPosition = Vector2.Lerp(mc_MainRectTransform.anchoredPosition, new Vector2(0, mf_MainCurrentPositionY), mf_Time);
        mc_ArrowRectTransform.rotation = Quaternion.Euler(mf_ArrowCurrentRotationX, 0, 0);
    }

    void OnDropQuest()
    {
        if (mc_MainRectTransform.anchoredPosition.y >= 360 - 10)
        {
            mf_MainCurrentPositionY = 0;
            mf_ArrowCurrentRotationX = 0;
        }
        else
        {
            mf_MainCurrentPositionY = 360;
            mf_ArrowCurrentRotationX = 180;
        }
    }

    void SetQuestDescription(string as_Description)
    {
        mc_DescriptionText.text = as_Description;
    }
}