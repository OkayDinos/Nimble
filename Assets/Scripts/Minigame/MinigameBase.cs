// This script belongs to Okay Dinos, used for the Nimble project.
// This script is used as a base for shared functions/ variables of minigame variations.
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Feb 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameBase : MonoBehaviour
{
    protected MinigameManager mr_MinigameManRef;

    protected InteractionPromptManager mr_IntProManRef;

    public void Start()
    {
        mr_MinigameManRef = MinigameManager.mr_Instance;

        mr_IntProManRef = InteractionPromptManager.mr_Instance;
    }

    public virtual void Begin()
    {
    }

    public virtual void SkipStep()
    {

    }
}
