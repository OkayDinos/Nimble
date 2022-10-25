// This script belongs to Okay Dinos, used for the Nimble Project.
// This script is used for the talk interaction.
// Author(s): Cyprian Przybyla, Morgan Finney.
// Date created - last edited: Feb 2022 - Apr 2022.

using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class MapLocation : Interactable
{
    [SerializeField] PlayerLocation mc_PlayerLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (MapController.mr_Instance)
            MapController.mr_Instance.DoSetMags(mc_PlayerLocation);
        }
    }
}