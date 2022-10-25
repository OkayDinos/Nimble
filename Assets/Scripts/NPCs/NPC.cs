using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public enum NPCStates
{
    Idle,
    Waving,
}

public class NPC : MonoBehaviour
{
    protected List<SpriteResolver> mc_Resolvers;

    protected LevelManager mr_LevelManRef;

    [SerializeField] protected Animator mc_Animator;

    [SerializeField] protected Talk mr_TalkRef;

    public NPCStates me_CurrentState;

    float mf_BlinkTimer;
    float mf_BlinkCD;

    private void Awake()
    {
        mc_Resolvers = GetComponentsInChildren<SpriteResolver>().ToList();

        mf_BlinkTimer = 0;
        mf_BlinkCD = Random.Range(0f, 4f);
    }

    private void Start()
    {
        mr_LevelManRef = LevelManager.mr_Instance;
    }

    protected void SetPart(string ms_ToSet)
    {
        foreach (var lc_Res in mc_Resolvers)
        {
            lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), ms_ToSet);
        }
    }

    protected void SetPartOverride(string ms_ToSet)
    {
        foreach (var lc_Res in mc_Resolvers)
        {
            var lb_Exists = lc_Res.spriteLibrary.GetSprite(lc_Res.GetCategory(), ms_ToSet);
            lc_Res.gameObject.SetActive(lb_Exists);
            if (lb_Exists) lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), ms_ToSet);
        }
    }

    protected void BlinkCheck()
    {
        mf_BlinkTimer += Time.deltaTime;
        if (mf_BlinkTimer > mf_BlinkCD)
        {
            mf_BlinkTimer = 0f;
            mf_BlinkCD = Random.Range(3f, 4f);

            Blink();
        }
    }

    protected virtual void Blink()
    {
    }
}