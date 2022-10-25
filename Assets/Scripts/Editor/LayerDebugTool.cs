// This script belongs to Okay Dinos, used for the Nimble Project.
// This script has various editor functions to help with sorting layers.
// Author(s): Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LayerDebugTool : EditorWindow
{
    int mi_SelectedIndex = 0;
    string[] ms_Layers = new string[] { "NONE", "FarGround", "DoveStreetHousesBack", "DoveHousesDynamics", "DoveHousesPlayer", "DoveStreetHousesFront", "DoveStreetRoadBack", "DoveStreetRaodFront", "DoveStreetPlayer", "MainStreetHousesBack", "ShoreHousesDynamics", "ShoreHousesFront", "MainStreetRoadBack", "ShoreRoadDynamics", "ShoreRoadPlayer", "MainStreetRoadFront", "LowerHarbourBack", "LowerHarbourFront", "Harbour", "HarbourWallBack", "HarbourWallFront", "NearGeound", "Debug", "Default" };

    int mi_SelectedIndexLayer = 0;
    List<SpriteRenderer> mc_SpriteRenders;

    [MenuItem("Nimble Tools/Layer Debug Tool")]
    static public void ShowWindow()
    {
        GetWindow(typeof(LayerDebugTool));
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Layer Debug Tool", EditorStyles.boldLabel);
        mi_SelectedIndex = EditorGUILayout.Popup(mi_SelectedIndex, ms_Layers);
        if (GUILayout.Button("Debug"))
            DoDebug();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Mass Sort", EditorStyles.boldLabel);
        if (GUILayout.Button("!!!   Mass Sort   !!!"))
            NewMassSort();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("Set Layers of Child", EditorStyles.boldLabel);
        mi_SelectedIndexLayer = EditorGUILayout.Popup(mi_SelectedIndexLayer, ms_Layers);
        if (GUILayout.Button("Set"))
            Sort();
    }

    void Sort()
    {
        GameObject[] lg_SelectedObjects = Selection.gameObjects;
        mc_SpriteRenders = new List<SpriteRenderer>();

        foreach (GameObject ag_GameObject in lg_SelectedObjects)
        {
            mc_SpriteRenders.AddRange(ag_GameObject.GetComponentsInChildren<SpriteRenderer>());
        }

        foreach (SpriteRenderer ac_SpriteRenderer in mc_SpriteRenders)
        {
            ac_SpriteRenderer.sortingLayerName = ms_Layers[mi_SelectedIndexLayer];
        }
    }

    void DoDebug()
    {
        GameObject lg_Debug = GameObject.FindGameObjectWithTag("Debug");

        for (int i = 0; i < ms_Layers.Length; i++)
        {
            Transform lg_GameObject = lg_Debug.transform.Find("Debug.Layers." + ms_Layers[i]);
            if (lg_GameObject != null)
            {
                if (i == mi_SelectedIndex)
                {
                    lg_GameObject.gameObject.SetActive(!lg_GameObject.gameObject.activeInHierarchy);
                }
                else
                {
                    lg_GameObject.gameObject.SetActive(false);
                }
            }
        }
    }
    void MassSort()
    {
        GameObject lg_Debug = GameObject.FindGameObjectWithTag("Debug");

        foreach (SpriteRenderer ac_SpriteRenderer in (SpriteRenderer[])UnityEngine.Object.FindObjectsOfType<SpriteRenderer>())
        {
            for (int i = 0; i < ms_Layers.Length; i++)
            {
                if (ms_Layers[i] != "NONE" && ms_Layers[i] != "Debug" && ms_Layers[i] != "Default")
                {
                    Transform first = lg_Debug.transform.Find("Debug.Layers." + ms_Layers[i]);
                    Transform second = lg_Debug.transform.Find("Debug.Layers." + ms_Layers[i + 1]);
                    if (second == null)
                    {
                        second = first;
                    }

                    if (ac_SpriteRenderer.transform.position.z < first.position.z && ac_SpriteRenderer.transform.position.z >= second.position.z)
                    {
                        ac_SpriteRenderer.sortingLayerName = ms_Layers[i];
                    }
                }
            }
        }
    }

    void NewMassSort()
    {
        foreach (SpriteRenderer ac_SpriteRenderer in (SpriteRenderer[])UnityEngine.Object.FindObjectsOfType<SpriteRenderer>())
        {
            if (ac_SpriteRenderer.transform.position.z > 11)
                ac_SpriteRenderer.sortingLayerName = "FarGround";
            else if (ac_SpriteRenderer.transform.position.z > 10 && ac_SpriteRenderer.transform.position.z <= 11)
                ac_SpriteRenderer.sortingLayerName = "DoveHousesDepthBack";
            else if (ac_SpriteRenderer.transform.position.z > 9 && ac_SpriteRenderer.transform.position.z <= 10)
                ac_SpriteRenderer.sortingLayerName = "DoveHousesDepthFront";
            else if (ac_SpriteRenderer.transform.position.z > 8 && ac_SpriteRenderer.transform.position.z <= 9)
                ac_SpriteRenderer.sortingLayerName = "DoveHousesBack";
            else if (ac_SpriteRenderer.transform.position.z >= 7 && ac_SpriteRenderer.transform.position.z <= 8)
                ac_SpriteRenderer.sortingLayerName = "DoveHousesFront";
            else if (ac_SpriteRenderer.transform.position.z > 6 && ac_SpriteRenderer.transform.position.z < 7)
                ac_SpriteRenderer.sortingLayerName = "DoveRoadBack";
            else if (ac_SpriteRenderer.transform.position.z > 5 && ac_SpriteRenderer.transform.position.z <= 6)
                ac_SpriteRenderer.sortingLayerName = "DoveRoadFront";
            else if (ac_SpriteRenderer.transform.position.z > 4 && ac_SpriteRenderer.transform.position.z <= 5)
                ac_SpriteRenderer.sortingLayerName = "ShoreHousesDepthBack";
            else if (ac_SpriteRenderer.transform.position.z > 3 && ac_SpriteRenderer.transform.position.z <= 4)
                ac_SpriteRenderer.sortingLayerName = "ShoreHousesDepthFront";
            else if (ac_SpriteRenderer.transform.position.z > 2 && ac_SpriteRenderer.transform.position.z <= 3)
                ac_SpriteRenderer.sortingLayerName = "ShoreHousesBack";
            else if (ac_SpriteRenderer.transform.position.z >= 1 && ac_SpriteRenderer.transform.position.z <= 2)
                ac_SpriteRenderer.sortingLayerName = "ShoreHousesFront";
            else if (ac_SpriteRenderer.transform.position.z > 0 && ac_SpriteRenderer.transform.position.z < 1)
                ac_SpriteRenderer.sortingLayerName = "ShoreRoadBack";
            else if (ac_SpriteRenderer.transform.position.z > -1 && ac_SpriteRenderer.transform.position.z <= 0)
                ac_SpriteRenderer.sortingLayerName = "ShoreRoadFront";
            else if (ac_SpriteRenderer.transform.position.z > -2 && ac_SpriteRenderer.transform.position.z <= -1)
                ac_SpriteRenderer.sortingLayerName = "LowerHarbourBack";
            else if (ac_SpriteRenderer.transform.position.z > -3 && ac_SpriteRenderer.transform.position.z <= -2)
                ac_SpriteRenderer.sortingLayerName = "LowerHarbourFront";
            else if (ac_SpriteRenderer.transform.position.z > -31 && ac_SpriteRenderer.transform.position.z <= -3)
                ac_SpriteRenderer.sortingLayerName = "Harbour";
            else if (ac_SpriteRenderer.transform.position.z > -32 && ac_SpriteRenderer.transform.position.z <= -31)
                ac_SpriteRenderer.sortingLayerName = "HarbourWallBack";
            else if (ac_SpriteRenderer.transform.position.z > -33 && ac_SpriteRenderer.transform.position.z <= -32)
                ac_SpriteRenderer.sortingLayerName = "HarbourWallFront";
            else if (ac_SpriteRenderer.transform.position.z <= -33)
                ac_SpriteRenderer.sortingLayerName = "NearGround";
            else
            {
                ac_SpriteRenderer.sortingLayerName = "Debug";
                Debug.LogWarning("Could Not Sort: " + ac_SpriteRenderer.gameObject.name);
            }
        }
    }
}