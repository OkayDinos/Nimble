// This script belongs to Okay Dinos, used for the Nimble project.
// This tool is used for switching scenes.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
public class SceneTool : EditorWindow
{
    protected const string ms_Load1 = "WorldTest";
    protected const string ms_Load2 = "EmiraTestScene";
    protected const string ms_Load3 = "LayerToolTestScene";
    protected const string ms_Load4 = "MainMenuTest";
    protected const string ms_Load5 = "MinigameTestScene";
    protected const string ms_Load6 = "SampleScene";

    [MenuItem("Scenes/" + ms_Load1)]
    public static void Load1()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load1 + ".unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/" + ms_Load2)]
    public static void Load2()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load2 + ".unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/" + ms_Load3)]
    public static void Load3()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load3 + ".unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/" + ms_Load4)]
    public static void Load4()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load4 + ".unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/" + ms_Load5)]
    public static void Load5()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load5 + ".unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/" + ms_Load6)]
    public static void Load6()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/Scenes/" + ms_Load6 + ".unity", OpenSceneMode.Single);
    }
}