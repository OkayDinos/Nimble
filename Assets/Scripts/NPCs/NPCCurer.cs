using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCurer : NPC
{
    void Start()
    {
        mc_Animator.Play("Idle");

        SetPartOverride("Resting");
        //SetPartOverride("Crossed");
    }

    void FixedUpdate()
    {
        if (me_CurrentState == NPCStates.Idle)
        {
            mc_Animator.Play("Idle");
        }
    }
}
