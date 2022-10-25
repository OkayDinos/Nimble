using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedYarnInput : MonoBehaviour
{
    [SerializeField]
    Yarn.Unity.LineView mc_LineView;

    void OnProgressDiolug()
    {
        //Debug.Log("PregressMe");
        //mc_LineView.UserRequestedViewAdvancement();
    }
}
