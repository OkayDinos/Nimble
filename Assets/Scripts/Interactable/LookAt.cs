// This script belongs to Okay Dinos, used for the Nimble project.
// This script inherits from interactable and lets the player look at/ examine things.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : Interactable
{
    public override void OnInteract()
    {
        base.OnInteract();

        mr_LevelManRef.FinishedInteracting();

        // TODO: Maybe use yarn spinner to let the player look at things similar to examine in dont starve? - CP
    }
}
