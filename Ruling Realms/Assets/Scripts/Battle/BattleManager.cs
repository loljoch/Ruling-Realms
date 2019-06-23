using ChrisTutorials.Persistent;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<int> playedIndexWhoPlayedJoker;
    private Castle targetedCastle;
    private int targetedPlayer;
    private int fireBalls;
    private List<Fireball> fireballList;
    private bool jokerMode;
    private int arrivedArmies;

    [SerializeField] Transform fireballParent;
    [SerializeField] SmokeFight smokeFight;
    [SerializeField] private AudioClip trumpetClip;
    [SerializeField] private AudioClip[] jokerClips;
    [SerializeField] private AudioClip cheeringClip;


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
        jokerMode = false;
        attackingPlayers = new List<PlayerAndValue>();
        defendingPlayer = new List<int>();
        playedIndexWhoPlayedJoker = new List<int>();
        fireballList = new List<Fireball>();
        foreach (Fireball fireball in fireballParent.GetComponentsInChildren<Fireball>())
        {
            fireballList.Add(fireball);
            fireball.ResetPositon();
        }
    }

    public void ManualCastleFocus(int playerIndex, Castle targetCastle)
    {
        targetedPlayer = playerIndex;
        targetedCastle = targetCastle;
    }

    public int[] GetPlayers()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            if (i != GameManager.Instance.currentPlayer.playerNumber)
            {
                tempList.Add(GameManager.Instance.playerList[i].playerNumber);
            }
        }

        return tempList.ToArray();
    }

    public int[] GetCastlesFromPlayer(int playerIndex)
    {
        targetedPlayer = playerIndex;
        List<int> castleIndexes = new List<int>();
        for (int i = 0; i < GameManager.Instance.playerList[playerIndex].castleList.Count; i++)
        {
            castleIndexes.Add(GameManager.Instance.playerList[playerIndex].castleList[i].castleIndex);
            
        }
        return castleIndexes.ToArray();
    }
    
    public void FocusCastle(int castleIndex)
    {
        targetedCastle = GameManager.Instance.playerList[targetedPlayer].castleList[castleIndex];
    }

    public void Attack(int playerIndex, int attackValue, int category)
    {
        if (!jokerMode)
        {
            if (category != 4)
            {
                int tempValue;
                if (((int)targetedCastle.category == category))
                {
                    if (attackValue != 1)
                    {
                        tempValue = attackValue * 2;//
                    } else
                    {
                        tempValue = attackValue;
                        attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], attackValue));
                    }
                } else
                {
                    tempValue = attackValue;
                }
                GameManager.Instance.playerList[playerIndex].army.armyCatergory = category;
                attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], tempValue));
            } else
            {
                attackingPlayers.Add(new PlayerAndValue(GameManager.Instance.playerList[playerIndex], 0));
                PlayJoker(playerIndex);
            }
        } else
        {
            playedIndexWhoPlayedJoker.RemoveAt(playedIndexWhoPlayedJoker.Count - 1);
            int tempValue = ((int)targetedCastle.category == category) ? attackValue * 2 : attackValue;
            SetJokerValue(playerIndex, tempValue);
        }
    }

    private void SetJokerValue(int playedIndex, int attackValue)
    {
        foreach (var joker in GameManager.Instance.playerList[playedIndex].army.jokers)
        {
            if (joker.gameObject.activeSelf)
            {
                if(joker.attackValue == -1)
                {
                    joker.attackValue = attackValue;
                    StartCoroutine(joker.GetComponent<Joker>().GrowToValue(attackValue));
                }
            }
        }
    }

    private void PlayJoker(int playedIndex)
    {
        playedIndexWhoPlayedJoker.Add(playedIndex);
    }

    public void Defend(int defendValue, int category)
    {
        if (category != 4)
        {
            int tempValue = ((int)targetedCastle.category == category) ? defendValue * 2 : defendValue;
            defendingPlayer.Add(tempValue);
        } else
        {
            PlayJoker(targetedPlayer);
            defendingPlayer.Add(defendValue);
        }
    }

    public void StartBattle()
    {
        for (int i = 0; i < attackingPlayers.Count; i++)
        {
            SpawnAttackingArmies(attackingPlayers[i]);
        }

        SpawnDefendingArmies(defendingPlayer);

        StartCoroutine(Battle());
    }

    private void SpawnAttackingArmies(PlayerAndValue playerAndValue)
    {
        Army currentArmy = playerAndValue.player.army;

        switch (playerAndValue.value)
        {
            case 0:
                currentArmy.SetArmyActive(currentArmy.jokers, 1, true);
                break;
            case 1:
                currentArmy.SetArmyActive(currentArmy.bannerman, 1, true);
                break;
            case 2:
                currentArmy.SetArmyActive(currentArmy.thiefs, 1, true);
                break;
            case 3:
                currentArmy.SetArmyActive(currentArmy.priests, 1, true);
                break;
            default:
                if (playerAndValue.value >= 10)
                {
                    for (int i = 0; i < playerAndValue.value; i += 10)
                    {
                        if (playerAndValue.value - i >= 10)
                        {
                            currentArmy.SetArmyActive(currentArmy.ogres, 1, true);
                        }
                    }
                }

                currentArmy.SetArmyActive(currentArmy.soldiers, playerAndValue.value % 10, true);
                break;
        }
    }

    private void SpawnDefendingArmies(List<int> values)
    {
        Army currentArmy = GameManager.Instance.playerList[targetedPlayer].army;
        List<int> deletedValues = new List<int>();
        int bigValue = 0;

        for (int i = 0; i < values.Count; i++)
        {
            if(values[i] > 3 && values[i] != 7)
            {
                bigValue += values[i];
                deletedValues.Add(values[i]);
            }
        }

        for (int i = 0; i < deletedValues.Count; i++)
        {
            values.Remove(deletedValues[i]);
        }

        if(bigValue > 0)
        {
            values.Add(bigValue);
        }

        for (int w = 0; w < values.Count; w++)
        {

            switch (values[w])
            {
                case 0:
                    currentArmy.SetArmyActive(currentArmy.jokers, 1, false);
                    break;
                case 1:
                    currentArmy.SetArmyActive(currentArmy.bannerman, 1, false);
                    break;
                case 2:
                    currentArmy.SetArmyActive(currentArmy.thiefs, 1, false);
                    break;
                case 3:
                    currentArmy.SetArmyActive(currentArmy.priests, 1, false);
                    break;
                case 7:
                    fireBalls++;
                    break;
                default:
                    if (values[w] >= 10)
                    {
                        for (int i = 0; i < values[w]; i += 10)
                        {
                            if (values[w] - i >= 10)
                            {
                                currentArmy.SetArmyActive(currentArmy.ogres, 1, false);
                            }
                        }
                    }

                    currentArmy.SetArmyActive(currentArmy.soldiers, values[w] % 10, false
    );
                    break;
            }
        }
    }

    private void RevealArmies(bool active)
    {
        List<Army> fightingArmies = GetFightingArmies();

        for (int i = 0; i < fightingArmies.Count; i++)
        {
            if (active)
            {
                fightingArmies[i].Reveal();
            } else
            {
                StartCoroutine(fightingArmies[i].Reset());
               
            }
        }
        
    }

    private void SendFireballDown(Army onArmy)
    {
        onArmy.isGettingFireballed = true;
        for (int i = 0; i < fireballList.Count; i++)
        {
            if (!fireballList[i].enabled)
            {
                fireballList[i].target = onArmy;
                fireballList[i].enabled = true;
                break;
            }
        }
    }

    private Army FindStrongestArmy()
    {
        Army currentStrongestArmy = null;

        List<Army> fightingArmies = GetFightingArmies();

        for (int i = 0; i < fightingArmies.Count; i++)
        {
            if (fightingArmies[i].fromPlayer != targetedPlayer)
            {
                int tempSize = fightingArmies[i].GetArmyStrength();
                if (tempSize != 0)
                {
                    try
                    {
                        if (tempSize >= currentStrongestArmy.GetArmyStrength() && !fightingArmies[i].isGettingFireballed)
                        {
                            currentStrongestArmy = fightingArmies[i];
                        }
                    } catch (System.NullReferenceException)
                    {

                        currentStrongestArmy = fightingArmies[i];
                    }

                }
            }
        }

        return currentStrongestArmy;
    }

    public List<Army> GetFightingArmies()
    {
        List<Army> fightingArmies = new List<Army>();

        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            if (GameManager.Instance.playerList[i].army.activeArmy.Count != 0)
            {
                fightingArmies.Add(GameManager.Instance.playerList[i].army);
            }
        }
        return fightingArmies;

    }

    IEnumerator Battle()
    {
        RevealArmies(true);
        yield return new WaitForSeconds(2);
        AudioManager.Instance.SetVolume(AudioManager.AudioChannel.Music, 50);
        if (fireBalls > 0)
        {
            for (int i = 0; i < fireBalls; i++)
            {
                defendingPlayer.Remove(7);
                SendFireballDown(FindStrongestArmy());
                yield return new WaitForSeconds(2);
            }
        }

        if (playedIndexWhoPlayedJoker.Count > 0)
        {
            AudioManager.Instance.Play(jokerClips[Random.Range(0, jokerClips.Length)], transform, 0.6f);
            jokerMode = true;
            playedIndexWhoPlayedJoker = playedIndexWhoPlayedJoker.Distinct().ToList();
            switch (playedIndexWhoPlayedJoker.Count)
            {
                case 1:
                    UiManager.Instance.BroadCastMessage(GameManager.Instance.playerList[playedIndexWhoPlayedJoker[0]].playerName + " may now assign a value to their joker", 3f);
                    break;
                case 2:
                    UiManager.Instance.BroadCastMessage(GameManager.Instance.playerList[playedIndexWhoPlayedJoker[0]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[1]].playerName + " may now assign a value to their joker", 3f);
                    break;
                case 3:
                    UiManager.Instance.BroadCastMessage(GameManager.Instance.playerList[playedIndexWhoPlayedJoker[0]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[1]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[2]].playerName + " may now assign a value to their joker", 3f);
                    break;
                case 4:
                    UiManager.Instance.BroadCastMessage(GameManager.Instance.playerList[playedIndexWhoPlayedJoker[0]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[1]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[2]].playerName
                        + " and " + GameManager.Instance.playerList[playedIndexWhoPlayedJoker[3]].playerName + " may now assign a value to their joker", 3f);
                    break;
                default:
                    break;
            }
            
        }
        if (playedIndexWhoPlayedJoker.Count > 0)
        {
            yield return new WaitUntil(() => playedIndexWhoPlayedJoker.Count <= 0);
            yield return new WaitForSeconds(2);
        }
        smokeFight.CalculateWinner();
        AudioManager.Instance.Play(trumpetClip, transform, 0.1f);
        smokeFight.audioSource.Play();
        yield return new WaitForSeconds(1);
        MarchArmies(true);
    }

    public void MarchArmies(bool moving)
    {
        List<Army> fightingArmies = GetFightingArmies();

        for (int i = 0; i < fightingArmies.Count; i++)
        {
            if (moving)
            {
                fightingArmies[i].StartMarching(true, Vector3.zero);
            } else
            {
                fightingArmies[i].StartMarching(false, Vector3.zero);

            }
        }
    }

    public IEnumerator BattleEnd(bool hasAttackersWon)
    {
        yield return new WaitForSeconds(2);
        if (!hasAttackersWon)
        {
            UiManager.Instance.BroadCastMessage("The defender has won the battle and will now gain the turn", 2);
            GameManager.Instance.nextPlayer = GameManager.Instance.playerList[targetedPlayer];
        } else
        {
            UiManager.Instance.BroadCastMessage("The attacking side has won the battle and " + GameManager.Instance.playerList[targetedPlayer].playerName + " has lost his castle!", 2);
            GameManager.Instance.DestroyCastle(targetedCastle);
        }

        GameManager.Instance.StartNextTurn();

        MarchArmies(false);
        AudioManager.Instance.Play(cheeringClip, transform, 0.2f);
        yield return new WaitForSeconds(0.5f);

        List<Army> fightingArmies = GetFightingArmies();
        for (int i = 0; i < fightingArmies.Count; i++)
        {
            fightingArmies[i].MakeArmyLookAt(Camera.main.transform.position, "Cheering");
        }

        smokeFight.audioSource.Stop();
        smokeFight.SetDustcloud(false);

        FireworkMachine fireworkMachine = FindObjectOfType<FireworkMachine>();
        List<Color32> playerColors = new List<Color32>();

        for (int i = 0; i < fightingArmies.Count; i++)
        {
            playerColors.Add(GameManager.Instance.playerList[fightingArmies[i].fromPlayer].playerColor);
        }

        StartCoroutine(fireworkMachine.FireFireworks(playerColors));
        yield return new WaitForSeconds(3);

        RevealArmies(false);
        ResetValues();
        yield return new WaitForSeconds(1);
        AudioManager.Instance.SetVolume(AudioManager.AudioChannel.Music, 100);
        smokeFight.ResetValues();
    }

    private void ResetValues()
    {
        attackingPlayers.Clear();
        defendingPlayer.Clear();
        playedIndexWhoPlayedJoker.Clear();
        targetedCastle = null;
        targetedPlayer = 6;
        fireBalls = 0;
        jokerMode = false;
        arrivedArmies = 0;
    }
}
