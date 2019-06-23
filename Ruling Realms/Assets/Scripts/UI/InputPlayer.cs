using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public string playerName { get; set; }
    public Color32 color;
    [SerializeField] private Color32[] colors;

    public void SetColor(int colorIndex)
    {
        color = colors[colorIndex];
    }
}
