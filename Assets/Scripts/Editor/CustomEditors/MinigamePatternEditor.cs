using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MinigamePattern))]
public class MinigamePatternEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MinigamePattern mr_MGPattern = target as MinigamePattern;

        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Grid"))
        {
            mr_MGPattern.mi_Pattern.Clear();

            for (int li_i = 0; li_i < mr_MGPattern.m2_PatternSize.y * mr_MGPattern.m2_PatternSize.x; li_i++)
            {
                mr_MGPattern.mi_Pattern.Add(-1);
            }
        }

        {
            GUILayout.BeginHorizontal();

            for (int li_i = 0; li_i < mr_MGPattern.m2_PatternSize.x; li_i++)
            {
                GUILayout.BeginVertical();

                for (int li_j = 0; li_j < mr_MGPattern.m2_PatternSize.y; li_j++)
                {
                    if (mr_MGPattern.mc_EditorIcon != null)
                    {
                        int li_Current = li_i + (li_j * (int)mr_MGPattern.m2_PatternSize.y);

                        if (GUILayout.Button($"{mr_MGPattern.mi_Pattern[li_Current]}"))
                        {
                            mr_MGPattern.mi_Pattern[li_Current]++;
                            if (mr_MGPattern.mi_Pattern[li_Current] > (mr_MGPattern.m4_Colours.Count - 1))
                            {
                                mr_MGPattern.mi_Pattern[li_Current] = -1;
                            }
                            Debug.Log($"{li_Current}");
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
    }
}