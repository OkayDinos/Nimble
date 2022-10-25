// This script belongs to Okay Dinos, used for the Nimble Project.
// This script can be used to swap the sprites.
// Author(s): Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

public class SpriteSwitcherTool : EditorWindow
{
    GameObject[] mg_SelectedObjects;
    bool mb_CheckChildren;
    List<SpriteRenderer> mc_SpriteRenders;
    [SerializeField] List<Sprite> mc_FromSprites, mc_ToSprites;
    Editor mr_Editor;

    [MenuItem("Nimble Tools/Sprite Switcher")]
    static public void ShowWindow()
    {
        GetWindow(typeof(SpriteSwitcherTool));
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite Switcher Tool", EditorStyles.boldLabel);

        if (!mr_Editor) { mr_Editor = Editor.CreateEditor(this); }
            if (mr_Editor) { mr_Editor.OnInspectorGUI(); }

        if (GUILayout.Button("Do Swap from Selected in Hierarchy"))
        {
            mg_SelectedObjects = Selection.gameObjects;
            SwitchSprites();
        }
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
            if(mb_CheckChildren)
            {
                mc_SpriteRenders.AddRange(ag_GameObject.GetComponentsInChildren<SpriteRenderer>());
            }
            if(ag_GameObject.GetComponent<SpriteRenderer>() != null)
            {
                mc_SpriteRenders.Add(ag_GameObject.GetComponent<SpriteRenderer>());
            }
        }
        foreach (SpriteRenderer ac_SpriteRenderer in mc_SpriteRenders)
        {
            if(mc_FromSprites.Contains(ac_SpriteRenderer.sprite))
            {
                ac_SpriteRenderer.sprite = mc_ToSprites[Random.Range(0, mc_ToSprites.Count)];
            }
        }
    }
}