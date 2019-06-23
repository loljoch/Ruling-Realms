using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Color playerColor;
    public string playerName;
    public int playerNumber;

    public List<Castle> castleList;
    public Transform cameraView;
    public Army army;
    public PlayerName playerNameUI;

    public Player(Color playerColor, string playerName, int playerNumber)
    {
        this.playerColor = playerColor;
        this.playerName = playerName;
        this.playerNumber = playerNumber;

        castleList = new List<Castle>();
    }

}
