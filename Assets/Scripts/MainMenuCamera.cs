using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MainMenuCameraMode { FollowBoat, Mags }

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] Transform mc_BoatCameraHolder, mc_MagsCameraHolder;
    Transform mc_CurrentHolder;
    MainMenuCameraMode me_CurrentCameraMode;

    private void Start()
    {
        me_CurrentCameraMode = MainMenuCameraMode.FollowBoat;
        UpdateCameraMode();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, mc_CurrentHolder.position, 0.05f);
    }

    private void SetCameraMode(MainMenuCameraMode ae_Mode)
    {
        me_CurrentCameraMode = ae_Mode;
        UpdateCameraMode();
    }

    private void UpdateCameraMode()
    {
        if(me_CurrentCameraMode == MainMenuCameraMode.FollowBoat)
        {
            mc_CurrentHolder = mc_BoatCameraHolder;
        }
        else if (me_CurrentCameraMode == MainMenuCameraMode.Mags)
        {
            mc_CurrentHolder = mc_MagsCameraHolder;
        }
    }
}