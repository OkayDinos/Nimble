using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGran : NPC
{
    void Start()
    {
        mc_Animator.Play("Idle");

        SetPartOverride("Resting");
    }

    void FixedUpdate()
    {
        if (me_CurrentState == NPCStates.Idle)
        {
            mc_Animator.Play("Idle");
        }
    }
}
