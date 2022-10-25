using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode { Default, Dove, House }

public class CameraController : MonoBehaviour
{
    GameObject mg_Target;
    [SerializeField]
    GameObject mg_PlayerCameraHolderMain, mg_PlayerCameraHolderDove, mg_PlayerCameraHolderHouse;
    Camera mc_Camera;


    private void Start()
    {
        mc_Camera = Camera.main;
        GameObject lg_Player = GameObject.FindGameObjectWithTag("Player");
        mg_Target = mg_PlayerCameraHolderHouse;
    }

    private void FixedUpdate()
    {
        mc_Camera.gameObject.transform.position = Vector3.Lerp(mc_Camera.gameObject.transform.position, mg_Target.transform.position, 0.1f);
    }

    public void SetTarget(GameObject ag_NewTarget)
    {
        mg_Target = ag_NewTarget;
    }

    public void ResetTarget(CameraMode ae_Mode)
    {
        switch (ae_Mode)
        {
            case CameraMode.Default:
                mg_Target = mg_PlayerCameraHolderMain;
                break;
            case CameraMode.Dove:
                mg_Target = mg_PlayerCameraHolderDove;
                break;
            case CameraMode.House:
                mg_Target = mg_PlayerCameraHolderHouse;
                break;
            default:
                break;
        }
    }
}