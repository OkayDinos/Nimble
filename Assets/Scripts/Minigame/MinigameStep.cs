// This script belongs to Okay Dinos, used for the Nimble project.
// This script is used for storing different steps used in cooking and fish prep minigames
// Author(s): Cyprian Przybyla.
// Date created - last edited: Feb 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nimble/Minigame/Step")]
public class MinigameStep : ScriptableObject
{
    [SerializeField] CookingLocation me_StepLocation;
    [SerializeField] CookingType me_StepType;
    [SerializeField] CookingIngredient me_StepIngredient;

    [SerializeField] MinigameObject mr_Object;
    [SerializeField] MinigameObject mr_Extra;
    [SerializeField] MinigameObject mr_Extra2;

    [SerializeField] string ms_HelpTextKeyboard;
    [SerializeField] string ms_HelpTextGamepad;
    [SerializeField] InteractionControl ms_HelpIconKeyboard;
    [SerializeField] InteractionControl ms_HelpIconGamepad;

    [SerializeField] Vector2 m2_CutZoneStartOffset;
    [SerializeField] Vector2 m2_CutZoneEndOffset;

    [SerializeField] public float mf_SoupLevel;

    [SerializeField] public bool mb_IsLastStir;
    [SerializeField] public bool mb_AddFire;

    public string GetHelpText(int ai_Type)
    {
        if (ai_Type == 0)
        {
            return ms_HelpTextKeyboard;
        }
        else
        {
            return ms_HelpTextGamepad;
        }
    }

    public InteractionControl GetHelpIcon(int ai_Type)
    {
        if (ai_Type == 0)
        {
            return ms_HelpIconKeyboard;
        }
        else
        {
            return ms_HelpIconGamepad;
        }
    }

    public void InitStep(CookingType ae_type, CookingIngredient ae_ingredient)
    {
        me_StepType = ae_type;
        me_StepIngredient = ae_ingredient;

        if (me_StepType == CookingType.AddCoal)
        {
            me_StepLocation = CookingLocation.OvenFront;
        }
        else if (me_StepType == CookingType.Stir || me_StepType == CookingType.Fry || me_StepType == CookingType.Mixing)
        {
            me_StepLocation = CookingLocation.StoveTop;
        }
        else
        {
            me_StepLocation = CookingLocation.TableTop;
        }
    }

    public CookingLocation GetStepLocation()
    {
        return me_StepLocation;
    }

    public CookingType GetStepType()
    {
        return me_StepType;
    }

    public CookingIngredient GetStepIngredient()
    {
        return me_StepIngredient;
    }
     
    public MinigameObject GetObject()
    {
        return mr_Object;
    }

    public MinigameObject GetExtra()
    {
        return mr_Extra;
    }

    public MinigameObject GetExtra2()
    {
        return mr_Extra2;
    }

    public Vector2 GetCutZoneOffset(bool ab_End)
    {
        if (!ab_End)
            return m2_CutZoneStartOffset;
        else
            return m2_CutZoneEndOffset;
    }
}
