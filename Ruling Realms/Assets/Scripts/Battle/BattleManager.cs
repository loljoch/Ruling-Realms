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
    private List<int> defendingPlayer;
    private List<int> amountOfJokerPlayedByPlayer;
    private Castle targetedCastle;
    private int targetedPlayer;
    private int fireBalls;

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

    private void Start()
    {
        attackingPlayers = new List<PlayerAndValue>();
        defendingPlayer = new List<int>();
        amountOfJokerPlayedByPlayer = new List<int>();
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
        if (category != 4)
        {
            int tempValue;
            if (((int)targetedCastle.category == category))
            {
                tempValue = attackValue * 2;
                if (tempValue == 0)
                {
                    attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], tempValue));
                }
            } else
            {
                tempValue = attackValue;
            }
            attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], tempValue));
        } else
        {
            PlayJoker(playerIndex);
        }
    }

    public void PlayJoker(int playedIndex)
    {
        amountOfJokerPlayedByPlayer.Add(playedIndex);
    }

    public void Defend(int defendValue, int category)
    {
        int tempValue = ((int)targetedCastle.category == category) ? defendValue * 2 : defendValue;
        defendingPlayer.Add(tempValue);
    }

    public void StartBattle()
    {
        for (int i = 0; i < attackingPlayers.Count; i++)
        {
            SpawnAttackingArmies(attackingPlayers[i]);
        }

        for (int i = 0; i < defendingPlayer.Count; i++)
        {
            SpawnDefendingArmies(defendingPlayer[i]);
        }

        RevealArmies();
    }

    private void SpawnAttackingArmies(PlayerAndValue playerAndValue)
    {
        Army currentArmy = playerAndValue.player.army;

        switch (playerAndValue.value)
        {
            case 0:
                currentArmy.SetArmyActive(currentArmy.bannerman, 1);
                break;
            case 2:
                currentArmy.SetArmyActive(currentArmy.thiefs, 1);
                break;
            case 3:
                currentArmy.SetArmyActive(currentArmy.priests, 1);
                break;
            default:
                if (playerAndValue.value >= 10)
                {
                    for (int i = 0; i < playerAndValue.value; i += 10)
                    {
                        currentArmy.SetArmyActive(currentArmy.ogres, 1);
                    }
                }

                currentArmy.SetArmyActive(currentArmy.soldiers, playerAndValue.value % 10);
                break;
        }
    }

    private void SpawnDefendingArmies(int value)
    {
        Army currentArmy = GameManager.Instance.playerList[targetedPlayer].army;

        switch (value)
        {
            case 0:
                currentArmy.SetArmyActive(currentArmy.bannerman, 1);
                break;
            case 2:
                currentArmy.SetArmyActive(currentArmy.thiefs, 1);
                break;
            case 3:
                currentArmy.SetArmyActive(currentArmy.priests, 1);
                break;
            case 7:
                fireBalls++;
                break;
            default:
                if (value >= 10)
                {
                    for (int i = 0; i < value; i += 10)
                    {
                        currentArmy.SetArmyActive(currentArmy.ogres, 1);
                    }
                }

                currentArmy.SetArmyActive(currentArmy.soldiers, value % 10);
                break;
        }
    }

    private void RevealArmies()
    {
        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            GameManager.Instance.playerList[i].army.Reveal();
        }
        
    }
}
