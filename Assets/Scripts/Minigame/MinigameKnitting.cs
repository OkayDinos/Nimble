using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MinigameKnitting : MinigameBase
{
    [SerializeField] List<MinigamePattern> mr_Patterns = new List<MinigamePattern>();

    MinigamePattern mr_CurrentPattern;
    Vector2 m2_PatternPosition;
    Vector2 m2_PatternSize;
    float mf_PatternTightness;
    List<Color> m4_PatternColours = new List<Color>();

    int mi_KnitCountTot;
    int mi_KnitCountCur;

    bool mb_IsKnitComplete;

    List<GameObject> mg_KnitList = new List<GameObject>();

    [SerializeField] GameObject mg_KnitObject;

    [SerializeField] GameObject mg_Knit;

    [SerializeField] GameObject mc_Slider;

    public new void Start()
    {
        base.Start();

        mr_CurrentPattern = mr_Patterns[GameManager.GetRecipeID()];
        m2_PatternSize = mr_CurrentPattern.m2_PatternSize;
        m4_PatternColours = mr_CurrentPattern.m4_Colours;
        m2_PatternPosition = new Vector2(0, 6f);
        mf_PatternTightness = 0.8f;

        mc_Slider.SetActive(false);
    }

    public async override void Begin()
    {
        mc_Slider.SetActive(true);
        mc_Slider.GetComponent<Slider>().value = 0;

        mi_KnitCountTot = 0;
        mi_KnitCountCur = 0;

        foreach (int li_Val in mr_CurrentPattern.mi_Pattern)
        {
            if (li_Val >= 0) mi_KnitCountTot++;
        }

        mb_IsKnitComplete = false;
        int li_i = 0; // counter
        foreach (int li_Value in mr_CurrentPattern.mi_Pattern)
        {
            if (li_Value != -1)
            {
                await DoKnitAction(m2_PatternPosition, li_Value, li_i);
                mb_IsKnitComplete = false;
            }
            m2_PatternPosition.x += mf_PatternTightness;
            if (m2_PatternPosition.x >= m2_PatternSize.x * mf_PatternTightness)
            {
                m2_PatternPosition.x = 0;
                m2_PatternPosition.y -= mf_PatternTightness - 0.2f;
            }
            li_i++;
        }
    }

    async Task DoKnitAction(Vector2 a2_Pos, int ai_ColourID, int ai_Layer)
    {
        while (mb_IsKnitComplete == false)
        {
            await Task.Yield();
        }

        AddKnit(a2_Pos, ai_ColourID, ai_Layer);
        mi_KnitCountCur++;
        mc_Slider.GetComponent<Slider>().value = (float)mi_KnitCountCur / (float)mi_KnitCountTot;
    }
    
    void AddKnit(Vector2 a2_Pos, int ai_ColourID, int ai_Layer)
    {
        GameObject lg_Object = Instantiate(mg_Knit);
        lg_Object.transform.parent = mg_KnitObject.transform;
        lg_Object.transform.position = new Vector3(a2_Pos.x, a2_Pos.y, 0f);
        lg_Object.GetComponent<SpriteRenderer>().sortingOrder = ai_Layer;
        lg_Object.GetComponent<SpriteRenderer>().color = m4_PatternColours[ai_ColourID];

        mg_KnitList.Add(lg_Object);
    }

    public override void SkipStep()
    {
        mb_IsKnitComplete = true;

        if (mi_KnitCountCur == mi_KnitCountTot)
        {
            mr_MinigameManRef.MinigameComplete(1f);
        }
    }
}
