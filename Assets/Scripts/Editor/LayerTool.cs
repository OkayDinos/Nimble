// This script belongs to Okay Dinos, used for the Nimble project.
// This tool is used for managing layers.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LayerTool : EditorWindow
{
    GameObject mg_ToSort;
    bool mb_SortChildren = false;

    struct LayerRange
    {
        public string LayerName;
        public float Min;
        public float Max;
    }

    readonly List<LayerRange> mr_Layers = new List<LayerRange>();

    [MenuItem("Nimble Tools/Old Layer Tool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LayerTool));
    }

    private void Awake() // Initialises all the layers tool, these values can be edited
    {
        float lf_Prev;
        AddLayer("FarGround", Mathf.Infinity, lf_Prev = 3f);
        AddLayer("DoveStreetBackground", lf_Prev, lf_Prev = 2f);
        AddLayer("DoveStreet", lf_Prev, lf_Prev = 1f);
        AddLayer("MainStreetBackground", lf_Prev, lf_Prev = 0f);
        AddLayer("MainStreet", lf_Prev, lf_Prev = -1f);
        AddLayer("HarbourWall", lf_Prev, lf_Prev = -2f);
        AddLayer("Test", lf_Prev, lf_Prev = -3f);
        AddLayer("NearGeound", lf_Prev, -Mathf.Infinity);
    }

    private void AddLayer(string as_Name, float af_Max, float af_Min)
    {
        LayerRange lr_Layer = new LayerRange
        {
            LayerName = as_Name,
            Min = af_Min,
            Max = af_Max,
        };
        mr_Layers.Add(lr_Layer);
    }

    private void OnGUI()
    {
        GUILayout.Label("Sprite layer sorting", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        EditorGUILayout.ObjectField("Select object", mg_ToSort, typeof(GameObject), true);

        if (Selection.activeTransform && Selection.count == 1)
            mg_ToSort = Selection.activeTransform.gameObject;
        else
            mg_ToSort = null;

        mb_SortChildren = EditorGUILayout.Toggle("Sort children", mb_SortChildren);

        EditorGUILayout.Space();

        if (GUILayout.Button("Refresh"))
        {
        }

        EditorGUI.BeginDisabledGroup(mg_ToSort == null || mg_ToSort != null && EditorUtility.IsPersistent(mg_ToSort));

        if (GUILayout.Button("Sort Selected"))
        {
            SortSelected(mg_ToSort);
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        if (Selection.activeObject != true)
        {
            EditorGUILayout.HelpBox("No object selected to sort.", MessageType.Warning);
        }

        if (mg_ToSort != null && EditorUtility.IsPersistent(mg_ToSort))
        {
            EditorGUILayout.HelpBox("Object selected cannot be a prefab", MessageType.Warning);
        }

        if (Selection.count > 1)
        {
            EditorGUILayout.HelpBox("Too many objects selected", MessageType.Warning);
        }
    }

    private void SortSelected(GameObject ag_ToSort)
    {
        if (ag_ToSort.GetComponent<SpriteRenderer>() != null)
        {
            SpriteRenderer lc_SpriteRenderer = ag_ToSort.GetComponent<SpriteRenderer>();
            float lf_Depth = ag_ToSort.transform.position.z;

            foreach (LayerRange ar_LR in mr_Layers)
            {
                if (InRange(lf_Depth, ar_LR.Min, ar_LR.Max))
                {
                    lc_SpriteRenderer.sortingLayerName = ar_LR.LayerName;
                }
            }
        }

        if (mb_SortChildren)
        {
            float lf_ChildCount = ag_ToSort.transform.childCount;

            for (int i = 0; i < lf_ChildCount; i++)
            {
                SortSelected(ag_ToSort.transform.GetChild(i).gameObject);
            }
        }

        static bool InRange(float af_Case, float af_Min, float af_Max)
        {
            if (af_Case > af_Min && af_Case <= af_Max)
                return true;
            else
                return false;
        }
    }
}
