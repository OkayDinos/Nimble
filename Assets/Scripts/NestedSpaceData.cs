// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used to hold data about nested spaces.
// Author(s): Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SortingLayerNames { NONE, FarGround, DoveStreetHousesBack, DoveStreetHousesFront, DoveStreetRoadBack, DoveStreetRaodFront, MainStreetHousesBack, MainStreetHousesFront, MainStreetRoadBack, MainStreetRoadFront, LowerHarbourBack, LowerHarbourFront, Harbour, HarbourWallBack, HarbourWallFront, NearGeound, Debug, Default};

[CreateAssetMenu(menuName = "Nimble/Nested Space Data")]
public class NestedSpaceData : ScriptableObject
{
    public int NestedSpaceSceneIndex;
    public SortingLayerNames NestedSpaceSortingLayerName;
    public SortingLayerNames ExitSortingLayerName;
    public LocationLayer NestedSpaceLocationLayer;
    public LocationLayer ExitLocationLayer;
    public string NestedSpaceName;
    public string ExitName;
    public string ExitSortingLayer;
    public string NestedSortingLayer;
    public float NestedSpacePlayerZPos;
    public float ExitPlayerZPos;
}
