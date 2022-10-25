// This script belongs to Okay Dinos, used for the Nimble project.
// This script inherits from interactable and lets the player change locations.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum InteractDirectionType
{
    Default,
    TowardsCam,
    AwayCam
}

public class MoveLocation : Interactable
{
    [SerializeField]
    public LocationLayer me_GoToLayer;

    [SerializeField]
    List<Transform> mc_Targets = new List<Transform>();

    [SerializeField]
    string ms_NewPlayerSortingLayerName;

    [SerializeField]
    InteractDirectionType me_InteractionType;

    [SerializeField]
    CameraMode me_NewCameraMode;

    [SerializeField]
    bool mb_IsGoingAway;

    public InteractDirectionType GetInteractionDirectionType()
    {
        return me_InteractionType;
    }

    public override void OnInteract()
    {
        base.OnInteract();

        mr_ActLogManRef.AddActionLog(GetAction(), NotificationType.Blue);

        mr_LevelManRef.mc_MainCamera.ResetTarget(me_NewCameraMode);

        StartCoroutine(mr_LevelManRef.MovePlayerMultiple(me_GoToLayer, mc_Targets, ms_NewPlayerSortingLayerName));
    }

    public override int TryWalkInt()
    {
        if (mb_IsGoingAway)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public string GetAction()
    {
        switch (me_GoToLayer)
        {
            case LocationLayer.MainStreet:
                return "You went to Shore street";
            case LocationLayer.HarbourWall:
                return "You went to harbour wall";
            case LocationLayer.DoveStreet:
                return "You went to Dove street";
            case LocationLayer.LowerHarbour:
                return "You went to lower harbour";
            default:
                return "You went back";
        }
    }

    // Prompt is decided based on layer - CP
    public override string GetPrompt()
    {
        switch (me_GoToLayer)
        {
            case LocationLayer.MainStreet:
                return "Go to Shore street";
            case LocationLayer.HarbourWall:
                return "Go to harbour wall";
            case LocationLayer.DoveStreet:
                return "Go to Dove street";
            case LocationLayer.LowerHarbour:
                return "Go to lower harbour";
            default:
                return "Go back";
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach(Transform tran in mc_Targets)
        {
            Handles.DrawAAPolyLine(transform.position, tran.position);
        }
    }
    #endif
}
