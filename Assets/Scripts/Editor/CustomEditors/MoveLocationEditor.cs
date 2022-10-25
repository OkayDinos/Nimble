using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveLocation))]
public class MoveLocationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set target Z"))
        {
            MoveLocation mr_base = (MoveLocation)target;
            Vector3 l3_NewPos = mr_base.gameObject.transform.GetChild(0).position;

            switch (mr_base.me_GoToLayer)
            {
                case LocationLayer.MainStreet:
                    l3_NewPos.z = 0f;
                    break;
                case LocationLayer.HarbourWall:
                    l3_NewPos.z = -66.5f;
                    break;
                case LocationLayer.DoveStreet:
                    l3_NewPos.z = 13;
                    break;
                case LocationLayer.LowerHarbour:
                    l3_NewPos.z = -13;
                    break;
                default:
                    l3_NewPos.z = 0;
                    break;
            }

            mr_base.gameObject.transform.GetChild(0).position = l3_NewPos;
        }
    }
}
