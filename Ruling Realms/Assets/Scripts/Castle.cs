using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Category
{
    public int fromPlayer;

    public void AssignCastle(Player player)
    {
        //GetComponent<Renderer>().material.color = player.playerColor;
        ColorBuildings(player.playerColor);
        fromPlayer = player.playerNumber;
    }

    private void ColorBuildings(Color32 color)
    {
        Renderer[] buildings = transform.parent.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < buildings.Length; i++)
        {
            for (int w = 0; w < buildings[i].materials.Length; w++)
            {
                if (buildings[i].materials[w].name == "PlayerCol (Instance)")
                {
                    buildings[i].materials[w].color = color;
                }
            }
        }
    }
}
