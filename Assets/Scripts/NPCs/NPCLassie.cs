using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLassie : NPC
{
    Pattern me_Pattern;
    Eyes me_Eyes;
    Nose me_Nose;
    Mouth me_Mouth;
    Face me_Face;
    Wrist me_Wrist;

    private void Start()
    {
        GenerateRandom();

        mc_Animator.Play("Idle");
    }

    private void FixedUpdate()
    {
        BlinkCheck();
    }

    protected override void Blink()
    {
        StartCoroutine(DoBlink());
    }

    IEnumerator DoBlink()
    {
        SetPart("Closed1");
        float lf_BlinkTimer = 0f;

        while (lf_BlinkTimer < 0.15f)
        {
            lf_BlinkTimer += Time.deltaTime;
            yield return null;
        }

         if (this != null) SetPart(me_Eyes.ToString());
    }

    public void GenerateRandom()
    {
        me_Pattern = (Pattern)Random.Range(0, 3);
        me_Eyes = (Eyes)Random.Range(0, 3);
        me_Nose = (Nose)Random.Range(0, 3);
        me_Mouth = (Mouth)Random.Range(0, 3);
        me_Face = (Face)Random.Range(0, 3);
        me_Wrist = (Wrist)Random.Range(0, 2);

        SetPart(me_Pattern.ToString());
        if (me_Pattern == Pattern.Pat3)
        {
            if (Random.value > 0.5f)
            {
                SetPart("Pat3Alt");
            }
        }
        SetPart(me_Eyes.ToString());
        SetPart(me_Nose.ToString());
        SetPart(me_Mouth.ToString());
        SetPart(me_Face.ToString());
        SetPart(me_Wrist.ToString());
    }

    enum Pattern { Pat1 = 0, Pat2, Pat3 }

    enum Eyes { Eyes1 = 0, Eyes2, Eyes3 }

    enum Nose { Nose1 = 0, Nose2, Nose3 }

    enum Mouth { Mouth1 = 0, Mouth2, Mouth3 }

    enum Face { Face1 = 0, Face2, Face3 }

    enum Wrist { Wrist1 = 0, Wrist2 }
}
