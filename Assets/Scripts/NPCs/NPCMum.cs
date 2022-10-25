using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using Yarn.Unity;
using Yarn.Unity.Example;

public class NPCMum : NPC
{
    [SerializeField] protected GameObject mg_EyeBone;

    [SerializeField] List<SpriteResolver> mc_EyeResolvers = new List<SpriteResolver>();

    [SerializeField] List<SpriteResolver> mc_MouthResolvers = new List<SpriteResolver>();
    
    float mf_NewBlinkTimer = 0f;
    float mf_NewBlinkCD = 1f;

    bool mb_IsTalking;

    [SerializeField] YarnCharacterView mc_YarnCharacter;

    [SerializeField] DialogueRunner mc_DialogueRunner;

    void Start()
    {
        mr_LevelManRef = LevelManager.mr_Instance;

        mc_Animator.Play("Idle");

        mb_IsTalking = false;

        if (this != null) NewSetPart("Silence", mc_MouthResolvers);
    }

    void FixedUpdate()
    {
        if (me_CurrentState == NPCStates.Idle)
        {
            mc_Animator.Play("Idle");
            MoveEyes(new Vector2(0.5f, -0.8f));
        }
        else
        {
            mc_Animator.Play("Waving");
            Vector3 l3_PlayerVector = (mr_LevelManRef.mg_PlayerRef.transform.position - transform.position).normalized;
            //l3_PlayerVector.x *= 0.5f;
            MoveEyes(l3_PlayerVector);
        }

        NewBlinkCheck();

        if (!mb_IsTalking && mc_YarnCharacter.GetSpeaker() == "Mother")
        {
            mb_IsTalking = true;
            Talk();
        }
    }

    protected void NewBlinkCheck()
    {
        mf_NewBlinkTimer += Time.deltaTime;
        if (mf_NewBlinkTimer > mf_NewBlinkCD)
        {
            mf_NewBlinkTimer = 0f;
            mf_NewBlinkCD = Random.Range(3f, 4f);

            NewBlink();
        }
    }

    async void NewBlink()
    {
        NewSetPartOverride("Closed", mc_EyeResolvers);
        float lf_BlinkTimer = 0f;

        while (lf_BlinkTimer < 0.15f)
        {
            lf_BlinkTimer += Time.deltaTime;
            await Task.Yield();
        }
        if (this != null) NewSetPartOverride("Open", mc_EyeResolvers);
    }

    protected void NewSetPart(string as_ToSet, List<SpriteResolver> ac_Resolvers)
    {
        foreach (var lc_Res in ac_Resolvers)
        {
            lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), as_ToSet);
        }
    }

    protected void NewSetPartOverride(string as_ToSet, List<SpriteResolver> ac_Resolvers)
    {
        foreach (var lc_Res in ac_Resolvers)
        {
            var lb_Exists = lc_Res.spriteLibrary.GetSprite(lc_Res.GetCategory(), as_ToSet);
            lc_Res.gameObject.SetActive(lb_Exists);
            if (lb_Exists) lc_Res.SetCategoryAndLabel(lc_Res.GetCategory(), as_ToSet);
        }
    }

    void MoveEyes(Vector2 a2_Pos) // Move eyes (Vector 2 goes from -1 to 1)
    {
        a2_Pos.x = Mathf.Clamp(a2_Pos.x, -1, 1);
        a2_Pos.y = Mathf.Clamp(a2_Pos.y, -1, 1);

        Vector2 l2_EyeCentre = new Vector2(0.23f, 0.065f);

        Vector2 l2_MaxOffset = new Vector2(0.02f, 0.02f);
        mg_EyeBone.transform.localPosition = l2_EyeCentre + new Vector2(a2_Pos.y * l2_MaxOffset.y, -a2_Pos.x * l2_MaxOffset.x);
    }

    protected override void Blink()
    {
        DoBlink();
    }

    async void DoBlink()
    {
        SetPartOverride("Closed");
        float lf_BlinkTimer = 0f;

        while (lf_BlinkTimer < 0.15f)
        {
            lf_BlinkTimer += Time.deltaTime;
            await Task.Yield();
        }

        if (this != null) SetPartOverride("Open");
    }

    async void Talk()
    {
        NewSetPart("Talk", mc_MouthResolvers);
        float lf_TalkTimer = 0f;
        float lf_TalkTime = 0.15f;
        bool lb_Open = true;

        while (mc_YarnCharacter.GetSpeaker() == "Mother" && mc_DialogueRunner.IsDialogueRunning)
        {
            lf_TalkTimer += Time.deltaTime;

            if (lf_TalkTimer > lf_TalkTime)
            {
                lf_TalkTimer = 0f;
                lb_Open = !lb_Open;
                if (lb_Open)
                {
                    if (this != null) NewSetPart("Talk", mc_MouthResolvers);
                    lf_TalkTime = Random.Range(0.05f, 0.2f);
                }
                else
                {
                    if (this != null) NewSetPart("Silence", mc_MouthResolvers);
                    lf_TalkTime = Random.Range(0.15f, 0.3f);
                }
            }

            await Task.Yield();
        }

        mb_IsTalking = false;
        if (this != null) NewSetPart("Silence", mc_MouthResolvers);
    }
}
