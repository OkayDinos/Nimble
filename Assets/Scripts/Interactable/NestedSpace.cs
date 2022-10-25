// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used for the nested space enter / exit interaction.
// Author(s): Morgan Finney.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

enum InteractionType { Enter, Exit}

public class NestedSpace : Interactable
{
    [SerializeField]
    NestedSpaceData mc_NestedSpaceData;

    [SerializeField]
    InteractionType me_InteractionType;

    [SerializeField]
    GameObject mg_CameraHolder;

    [SerializeField]
    GameObject mg_House;

    public override void OnInteract()
    {
        base.OnInteract();

        GameObject lg_Player = mr_LevelManRef.mg_PlayerRef;
        Vector3 l3_Position;

        //* Load scene wait for callback
        //* Then fade sprite and light and start to move.




        switch (me_InteractionType)
        {
            case InteractionType.Enter:
                StartCoroutine(LoadNestedSpace());
                break;
            case InteractionType.Exit:
                StartCoroutine(UnLoadNestedSpace());
                break;
            default:
                break;
        }
    }

    IEnumerator LoadNestedSpace()
    {
        GameObject lg_Player = mr_LevelManRef.mg_PlayerRef;
        Vector3 l3_Position;


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mc_NestedSpaceData.NestedSpaceSceneIndex, LoadSceneMode.Additive);

        yield return new WaitUntil(() => asyncLoad.isDone == true);

        mr_LevelManRef.UpdateLayers();


        l3_Position = new Vector3(lg_Player.transform.position.x, lg_Player.transform.position.y, mc_NestedSpaceData.NestedSpacePlayerZPos);
        mr_LevelManRef.MovePlayer(mc_NestedSpaceData.NestedSpaceLocationLayer, l3_Position, mc_NestedSpaceData.NestedSortingLayer);

        mr_LevelManRef.mc_MainCamera.ResetTarget(CameraMode.House);

        mr_LevelManRef.GetSetNestedHouse(mg_House);

        float lf_Decoy = 1f;
        while (lf_Decoy > 0.02f)
        {
            yield return new WaitForEndOfFrame();

            lf_Decoy = Mathf.Lerp(lf_Decoy, 0, 0.4f);

            mg_House.GetComponentInChildren<SpriteRenderer>().sharedMaterial.SetFloat("_Opacity", lf_Decoy);

            mr_LevelManRef.SendMessage("ToggleLight", lf_Decoy);
        }

        yield return new WaitForEndOfFrame();
        mr_LevelManRef.GetSetNestedHouse().GetComponentInChildren<SpriteRenderer>().sharedMaterial.SetFloat("_Opacity", 0);
    }

    IEnumerator UnLoadNestedSpace()
    {
        GameObject lg_Player = mr_LevelManRef.mg_PlayerRef;
        Vector3 l3_Position;

        l3_Position = new Vector3(lg_Player.transform.position.x, lg_Player.transform.position.y, mc_NestedSpaceData.ExitPlayerZPos);
        mr_LevelManRef.MovePlayer(mc_NestedSpaceData.ExitLocationLayer, l3_Position, mc_NestedSpaceData.ExitSortingLayer);

        mr_LevelManRef.mc_MainCamera.ResetTarget(CameraMode.Dove);

        float lf_Decoy = 0f;

        //* GHETTO CODE
        //* Should replace this

        while (lf_Decoy < 0.98f)
        {
            yield return new WaitForEndOfFrame();

            lf_Decoy = Mathf.Lerp(lf_Decoy, 1, 0.4f);

            GameObject lg_House = mr_LevelManRef.GetSetNestedHouse();

            lg_House.GetComponentInChildren<SpriteRenderer>().sharedMaterial.SetFloat("_Opacity", lf_Decoy);

            mr_LevelManRef.SendMessage("ToggleLight", lf_Decoy);
        }

        yield return new WaitForEndOfFrame();
        mr_LevelManRef.GetSetNestedHouse().GetComponentInChildren<SpriteRenderer>().sharedMaterial.SetFloat("_Opacity", 1);
        SceneManager.UnloadSceneAsync(mc_NestedSpaceData.NestedSpaceSceneIndex);
    }

    public override string GetPrompt()
    {
        switch (me_InteractionType)
        {
            case InteractionType.Enter:
                return "To enter " + mc_NestedSpaceData.NestedSpaceName;
            case InteractionType.Exit:
                return "To leave to " + mc_NestedSpaceData.ExitName;
            default:
                return "To enter " + mc_NestedSpaceData.NestedSpaceName;
        }
    }
}
