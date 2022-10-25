using UnityEditor;
using UnityEngine;

public class CameraTool : EditorWindow
{
    GameObject mg_Selected;

    [MenuItem("Nimble Tools/Camera")]
    static public void ShowWindow()
    {
        GetWindow(typeof(CameraTool));
    }

    private void OnGUI()
    {
        EditorGUILayout.ObjectField("Select object", mg_Selected, typeof(GameObject), true);

        if (Selection.activeObject && Selection.count == 1)
        {
            if (Selection.activeTransform.gameObject.GetComponent<Camera>() != null)
            mg_Selected = Selection.activeTransform.gameObject;
        }
        else
            mg_Selected = null;

        EditorGUI.BeginDisabledGroup(mg_Selected == null || mg_Selected != null && EditorUtility.IsPersistent(mg_Selected));

        if (GUILayout.Button("Perspective sorting"))
        {
            mg_Selected.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Perspective;
        }

        if (GUILayout.Button("Orthographic sorting"))
        {
            mg_Selected.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
        }

        EditorGUI.EndDisabledGroup();

        if (Selection.activeObject != true)
        {
            EditorGUILayout.HelpBox("No camera selected.", MessageType.Warning);
        }

        if (mg_Selected != null && EditorUtility.IsPersistent(mg_Selected))
        {
            EditorGUILayout.HelpBox("Object selected cannot be a prefab.", MessageType.Warning);
        }

        if (Selection.count > 1)
        {
            EditorGUILayout.HelpBox("Too many things selected.", MessageType.Warning);
        }

        if (mg_Selected != null && Selection.activeTransform.gameObject.GetComponent<Camera>() == null)
        {
            EditorGUILayout.HelpBox("Object selected not a camera.", MessageType.Warning);
        }
    }
}