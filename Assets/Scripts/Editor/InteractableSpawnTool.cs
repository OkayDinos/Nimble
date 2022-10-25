// This script belongs to Okay Dinos, used for the Nimble project.
// This tool is used for spawning interactables;
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using UnityEditor;
using UnityEngine;

public enum InteractableType
{
    LookAt,
    Minigame,
    Move,
    Pickup,
    Talk,
}

public class InteractableSpawnTool : EditorWindow
{
    [MenuItem("Nimble Tools/Interactable Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(InteractableSpawnTool));
    }

    InteractableType me_Type;

    LocationLayer me_SpawnAtLayer;

    LocationLayer me_GoToLayer;

    GameObject mg_LookAt;
    GameObject mg_Minigame;
    GameObject mg_Move;
    GameObject mg_PickUp;
    GameObject mg_Talk;

    string ms_LayerName;

    private void Awake()
    {
        GameObject lg_SpawnList = GameObject.Find("SpawnList");

        mg_LookAt = lg_SpawnList.GetComponent<SpawnList>().mg_LookAt;
        mg_Minigame = lg_SpawnList.GetComponent<SpawnList>().mg_Minigame;
        mg_Move = lg_SpawnList.GetComponent<SpawnList>().mg_Move;
        mg_PickUp = lg_SpawnList.GetComponent<SpawnList>().mg_PickUp;
        mg_Talk = lg_SpawnList.GetComponent<SpawnList>().mg_Talk;
    }

    private void OnGUI()
    {
        me_Type = (InteractableType)EditorGUILayout.EnumPopup("Spawn type:", me_Type);

        GUILayout.Space(10);

        me_SpawnAtLayer = (LocationLayer)EditorGUILayout.EnumPopup("Spawn in layer:", me_SpawnAtLayer);

        GUILayout.Space(10);

        if (me_Type == InteractableType.Move)
        {
            GUILayout.Label("Move options:");

            me_GoToLayer = (LocationLayer)EditorGUILayout.EnumPopup("Got To Layer:", me_GoToLayer);

            GUILayout.Space(10);
        }

        if (GUILayout.Button("+ Spawn Interactable"))
        {
            SpawnInteractable();
        }
    }

    private void SpawnInteractable()
    {
        Vector3 l3_SpawnPos = Vector3.zero;

        switch (me_SpawnAtLayer)
        {
            case LocationLayer.MainStreet:
                ms_LayerName = "Main Street";
                l3_SpawnPos.z = 0f;
                break;
            case LocationLayer.HarbourWall:
                ms_LayerName = "Harbour Wall";
                l3_SpawnPos.z = -66.5f;
                break;
            case LocationLayer.DoveStreet:
                ms_LayerName = "Dove Street";
                l3_SpawnPos.z = 13f;
                break;
            case LocationLayer.LowerHarbour:
                ms_LayerName = "Lower Harbour";
                l3_SpawnPos.z = -13f;
                break;
            case LocationLayer.Inside:
                ms_LayerName = "Inside";
                l3_SpawnPos.z = 0f;
                break;
            default:
                ms_LayerName = "Main Street";
                l3_SpawnPos.z = 0f;
                break;
        }

        GameObject lg_ToSpawn;

        switch (me_Type)
        {
            case InteractableType.LookAt:
                lg_ToSpawn = mg_LookAt;
                break;
            case InteractableType.Minigame:
                lg_ToSpawn = mg_Minigame;
                break;
            case InteractableType.Move:
                lg_ToSpawn = mg_Move;
                break;
            case InteractableType.Pickup:
                lg_ToSpawn = mg_PickUp;
                break;
            case InteractableType.Talk:
                lg_ToSpawn = mg_Talk;
                break;
            default:
                lg_ToSpawn = mg_LookAt;
                break;
        }

        GameObject lg_SpawnFrom = GameObject.Find("Interactables/" + ms_LayerName);

        GameObject lg_New = (GameObject)PrefabUtility.InstantiatePrefab(lg_ToSpawn);

        lg_New.transform.position = l3_SpawnPos;

        lg_New.name = lg_New.name.Replace("(Clone)", "");

        lg_New.transform.parent = lg_SpawnFrom.transform;

        lg_New.GetComponent<Interactable>().SetLayer(me_SpawnAtLayer);

        if (me_Type == InteractableType.Move)
        {
            lg_New.GetComponent<MoveLocation>().me_GoToLayer = me_GoToLayer;

            Vector3 l3_TargetPos = Vector3.zero;

            switch (me_GoToLayer)
            {
                case LocationLayer.MainStreet:
                    l3_TargetPos.z = 0f;
                    break;
                case LocationLayer.HarbourWall:
                    l3_TargetPos.z = -66.5f;
                    break;
                case LocationLayer.DoveStreet:
                    l3_TargetPos.z = 13;
                    break;
                case LocationLayer.LowerHarbour:
                    l3_TargetPos.z = -13;
                    break;
                default:
                    l3_TargetPos.z = 0;
                    break;
            }

            lg_New.gameObject.transform.GetChild(0).transform.position = l3_TargetPos;
        }
    }
}
