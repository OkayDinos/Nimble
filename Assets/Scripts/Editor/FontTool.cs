// This script belongs to Okay Dinos, used for the Nimble project.
// This script find existing `TextMeshProUGUI` components with a specified `TMP_FontAsset` and replaces that reference with another specified `TMP_FontAsset`.
// Author(s): Morgan Finney [pdox].
// Date created - last edited: Mar 2021 – Mar 2022.

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class FontTool : EditorWindow
{
    GameObject[] mg_SelectedObjects;
    List<TextMeshProUGUI> mc_TMP;
    [SerializeField] TMP_FontAsset mc_FromFont, mc_ToFont;
    Editor mr_Editor;

    [MenuItem("Nimble Tools/Font Tool")]
    static public void ShowWindow()
    {
        GetWindow(typeof(FontTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Font Switcher Tool", EditorStyles.boldLabel);

        if (!mr_Editor) { mr_Editor = Editor.CreateEditor(this); }
        if (mr_Editor) { mr_Editor.OnInspectorGUI(); }

        if (GUILayout.Button("↕ Switch"))
        {
            SwitchFont();
        }

        if (GUILayout.Button("Swap All Fonts"))
        {
            List<GameObject> ag_GameObjects = new List<GameObject>();

            foreach (TextMeshProUGUI ac_TMP in (TextMeshProUGUI[])UnityEngine.Object.FindObjectsOfType<TextMeshProUGUI>())
            {
                ag_GameObjects.Add(ac_TMP.gameObject);
            }

            mg_SelectedObjects = ag_GameObjects.ToArray();
            SwitchSprites();
        }
    }

    void SwitchFont()
    {
        TMP_FontAsset l_Temp = mc_FromFont;
        mc_FromFont = mc_ToFont;
        mc_ToFont = l_Temp;
    }

    void SwitchSprites()
    {
        mc_TMP = new List<TextMeshProUGUI>();

        foreach (GameObject ag_GameObject in mg_SelectedObjects)
        {
            if (ag_GameObject.GetComponent<TextMeshProUGUI>() != null)
            {
                mc_TMP.Add(ag_GameObject.GetComponent<TextMeshProUGUI>());
            }
        }

        foreach (TextMeshProUGUI ac_TMP in mc_TMP)
        {
            if (mc_FromFont == ac_TMP.font)
            {
                ac_TMP.font = mc_ToFont;
            }
        }
    }
}