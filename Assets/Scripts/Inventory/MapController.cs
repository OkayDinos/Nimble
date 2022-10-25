using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerLocation { Home, DoveSection01, DoveSection02, DoveSection03, ShoreSection01, ShoreSection02, ShoreSection03, ShoreSection04, LowerSection01, LoverSection02, LowerSection03, WallSection01, WallSection02 }

public class MapController : MonoBehaviour
{
    public static MapController mr_Instance;

    PlayerLocation me_CurrentLocation = PlayerLocation.Home;

    private void Awake()
    {
        if (mr_Instance != null)
            Destroy(mr_Instance);
        mr_Instance = this;
    }

    public void DoUpdateMap()
    {
        foreach (MapElement ac_MapElement in GetComponentsInChildren<MapElement>())
        {
            ac_MapElement.DoUpdateElement();
        }

        foreach (MagsMapElement ac_MapElement in GetComponentsInChildren<MagsMapElement>())
        {
            ac_MapElement.DoUpdateElement(me_CurrentLocation);
        }
    }

    public void DoSetMags(PlayerLocation ae_PlayerLocation)
    {
        me_CurrentLocation = ae_PlayerLocation;
    }
}
