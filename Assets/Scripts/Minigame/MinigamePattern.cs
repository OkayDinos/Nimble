using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Nimble/Minigame/Pattern")]
public class MinigamePattern : ScriptableObject
{
    public Texture mc_EditorIcon;

    public List<Color> m4_Colours = new List<Color>();

    public Vector2 m2_PatternSize;

    [SerializeField] public List<int> mi_Pattern = new List<int>();
}