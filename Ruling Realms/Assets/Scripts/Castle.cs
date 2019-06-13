using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Category
{
    public int fromPlayer;

    public void AssignCastle(Player player)
    {
        GetComponent<Renderer>().material.color = player.playerColor;
        fromPlayer = player.playerNumber;
    }
}
