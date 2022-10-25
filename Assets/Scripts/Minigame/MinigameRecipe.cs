// This script belongs to Okay Dinos, used for the Nimble project.
// This scriptable object is used for storing recipes
// Author(s): Cyprian Przybyla.
// Date created - last edited: Mar 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nimble/Minigame/Recipe")]
public class MinigameRecipe : ScriptableObject
{
    int mi_CurrentStep;

    [SerializeField] List<MinigameStep> mr_steps = new List<MinigameStep>();

    [SerializeField] Sprite mc_Final;

    [SerializeField] Sprite mc_FinalExtra;

    public void Begin()
    {
        mi_CurrentStep = 0;
    }

    public MinigameStep GetCurrentStep()
    {
        return mr_steps[mi_CurrentStep];
    }

    public bool IncrementStep()
    {
        mi_CurrentStep++;
        if (mi_CurrentStep > (mr_steps.Count - 1))
        {
            return false;
        }
        return true;
    }

    public Sprite GetFinal()
    {
        return mc_Final;
    }

    public Sprite GetFinalExtra()
    {
        return mc_FinalExtra;
    }
}
