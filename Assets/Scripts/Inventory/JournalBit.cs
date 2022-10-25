using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum JournalContentType { Image, Text }

public class JournalBit : MonoBehaviour
{
    [SerializeField]
    ItemData mr_Note;

    [SerializeField]
    JournalContentType me_Type;

    [SerializeField] [TextArea]
    string ms_Text;

    [SerializeField]
    Image mc_Image;

    [SerializeField]
    TextMeshProUGUI mc_TMP;

    void DoCheck()
    {
        switch (me_Type)
        {
            case JournalContentType.Image:
                mc_TMP.enabled = false;

                if (ItemDatabase.CheckHas(mr_Note))
                    mc_Image.color = Color.white;
                else
                    mc_Image.color = new Color(0.2f,0.2f,0.2f,0.5f);
                break;
            case JournalContentType.Text:
                mc_Image.enabled = false;

                if (ItemDatabase.CheckHas(mr_Note))
                    mc_TMP.text = ms_Text;
                else
                    mc_TMP.text = $"<alpha=#88><s>{ms_Text}";
                break;
            default:
                break;
        }


    }
}
