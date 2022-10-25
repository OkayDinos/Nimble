// This script belongs to Okay Dinos, used for the Nimble project.
// This script adds a specified material to all selected game objects and their children.
// Author(s): Morgan Finney [pdox].
// Date created - last edited: Mar 2022 – Mar 2022.

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class MatSwitcher : EditorWindow
{
    GameObject[] mg_SelectedObjects;
    List<SpriteRenderer> mc_SpriteRenders;
    [SerializeField] Material mate;
    Editor mr_Editor;

    [MenuItem("Nimble Tools/Mat Switcher")]
    static public void ShowWindow()
    {
        GetWindow(typeof(MatSwitcher));
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Switcher Tool", EditorStyles.boldLabel);

        if (!mr_Editor) { mr_Editor = Editor.CreateEditor(this); }
            if (mr_Editor) { mr_Editor.OnInspectorGUI(); }

        if (GUILayout.Button("!!!      Do Swap on All      !!!"))
        {
            List<GameObject> ag_GameObjects = new List<GameObject>();

            foreach (SpriteRenderer ac_SpriteRenderer in (SpriteRenderer[])UnityEngine.Object.FindObjectsOfType<SpriteRenderer>())
            {
                ag_GameObjects.Add(ac_SpriteRenderer.gameObject);
            }
            mg_SelectedObjects = ag_GameObjects.ToArray();
            SwitchSprites();
        }
    }

    void SwitchSprites()
    {
        mc_SpriteRenders = new List<SpriteRenderer>(); 

        foreach (GameObject ag_GameObject in mg_SelectedObjects)    
        {
            mc_SpriteRenders.AddRange(ag_GameObject.GetComponentsInChildren<SpriteRenderer>());

            if(ag_GameObject.GetComponent<SpriteRenderer>() != null)
            {
                mc_SpriteRenders.Add(ag_GameObject.GetComponent<SpriteRenderer>());
            }
        }
        foreach (SpriteRenderer ac_SpriteRenderer in mc_SpriteRenders)
        {
                ac_SpriteRenderer.sharedMaterial = mate;
        }
    }
}