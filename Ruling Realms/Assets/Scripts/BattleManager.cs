using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private static BattleManager instance;
    public static BattleManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    private struct PlayerAndValue
    {
        public Player player;
        public int value;

        public PlayerAndValue(Player player, int value)
        {
            this.player = player;
            this.value = value;
        }
    }

    private List<PlayerAndValue> attackingPlayers;
    private Castle targetedCastle;
    private int targetedPlayer;

    private void Awake()
    {
        Instance = null;
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    public string[] GetPlayers()
    {
        List<string> tempList = new List<string>();

        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            if (i != GameManager.Instance.currentPlayer.playerNumber)
            {
                tempList.Add(GameManager.Instance.playerList[i].playerName);
            }
        }

        return tempList.ToArray();
    }

    public int GetCastlesFromPlayer(int playerIndex)
    {
        targetedPlayer = playerIndex;
        return GameManager.Instance.playerList[playerIndex].castleList.Count;
    }
    
    public void FocusCastle(int castleIndex)
    {
        targetedCastle = GameManager.Instance.playerList[targetedPlayer].castleList[castleIndex];
    }

    public void Attack(int playerIndex, int attackValue, int category)
    {
        int tempValue = ((int)targetedCastle.category == category) ? attackValue * 2 : attackValue;
        attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], attackValue));


    }
}
