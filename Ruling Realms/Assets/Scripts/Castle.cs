using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Category
{
    public int fromPlayer;
    public int castleIndex;

    public void AssignCastle(Player player, int castleIndex)
    {
        ColorBuildings(player.playerColor);
        fromPlayer = player.playerNumber;
        this.castleIndex = castleIndex;
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
