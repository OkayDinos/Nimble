// This script belongs to Okay Dinos, used for the Nimble project.
// This scriptable object is used for storing objects
// Author(s): Cyprian Przybyla.
// Date created - last edited: Mar 2022 - Mar 2022.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nimble/Minigame/Object")]
public class MinigameObject : ScriptableObject
{
    [SerializeField] Sprite mc_Default;
    [SerializeField] Sprite mc_Mashed;
    [SerializeField] Sprite mc_Cut;
    [SerializeField] Sprite mc_Cut2;
    [SerializeField] Sprite mc_MashedFried;

    [SerializeField] List<Sprite> mc_Levels = new List<Sprite>();

    [SerializeField] float mf_DefaultSize;

    public bool mb_IsCut;
    public bool mb_IsCut2;
    public bool mb_IsMashed;
    public bool mb_IsFried;

    public int mi_Layer;

    public Vector2 m2_ScreenOffset;

    private void Awake()
    {
        Reset();
    }

    public float GetDefaultSize()
    {
        return mf_DefaultSize;
    }

    public void Reset()
    {
        mb_IsCut = false;
        mb_IsCut2 = false;
        mb_IsMashed = false;
        mb_IsFried = false;
    }

    public Sprite GetSprite(float af_level = -1)
    {
        if (!(af_level == -1))
        {
            for (int li_i = 0; li_i < mc_Levels.Count; li_i++)
            {
                if (((mc_Levels.Count - li_i) / (mc_Levels.Count + 1f)) < af_level)
                {
                    return mc_Levels[li_i];
                }
            }
        }

        if (mb_IsMashed)
        {
            if (mb_IsFried)
            {
                return mc_MashedFried;
            }

            return mc_Mashed;
        }
        else if (mb_IsCut2)
        {
            return mc_Cut2;
        }
        else if (mb_IsCut)
        {
            return mc_Cut;
        }
        else
        {
            return mc_Default;
        }
    }
}