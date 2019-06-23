using ChrisTutorials.Persistent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    private const int maxPlayers = 4;
    public const float startGameWaitingTime = 1;

    [HideInInspector] public List<Player> playerList;

    [SerializeField] private List<GameObject> playerItemList;
    [SerializeField] private AudioClip nextPlayerClip;
    [SerializeField] private AudioClip castleDestroyClip;
    [SerializeField] private AudioClip assignCategoryClip;
    [SerializeField] private AudioClip bgMusic;

    private CameraView camera;

    public Player currentPlayer;
    public Player nextPlayer;

    private void Awake()
    {
        Instance = null;
        if(Instance == null)
        {
            Instance = this;
        } else if(Instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        playerList = new List<Player>();
        camera = Camera.main.GetComponent<CameraView>();
        AudioManager.Instance.PlayLoop(bgMusic, transform, 0.3f, 1, true);
    }

    public void AssignPlayer(Color32 rgb, string playerName)
    {

        if (playerList.Count < maxPlayers)
        {
            Color32 tempPlayerColor = rgb;
            playerList.Add(new Player(tempPlayerColor, playerName, playerList.Count));

            if (UiManager.Instance.playersJoinedMenu.gameObject.activeSelf)
            {
                UiManager.Instance.playersJoinedMenu.ShowJoinedPlayers();
            }
        } else
        {
            UiManager.Instance.playersJoinedMenu.StartCountDown();
            AssignItemsToPlayers();
        }

    }

    //public void AssignPlayer(string data)
    //{
    //    var jsonClass = JsonUtility.FromJson<JsonClass>(data);

    //    if (playerList.Count < maxPlayers)
    //    {
    //        Color32 tempPlayerColor = new Color32((byte)jsonClass.color[0], (byte)jsonClass.color[1], (byte)jsonClass.color[2], 255);
    //        playerList.Add(new Player(tempPlayerColor, jsonClass.name, playerList.Count));

    //        if (UiManager.Instance.playersJoinedMenu.gameObject.activeSelf)
    //        {
    //            UiManager.Instance.playersJoinedMenu.ShowJoinedPlayers();
    //        }
    //    } else
    //    {
    //        UiManager.Instance.playersJoinedMenu.StartCountDown();
    //        AssignItemsToPlayers();
    //    }

    //}

    private void AssignItemsToPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            int castleIndex = 0;
            Transform[] tempItemArray = playerItemList[i].GetComponentsInChildren<Transform>();
            playerList[i].playerNameUI = playerItemList[i].GetComponentInChildren<PlayerName>();
            playerItemList[i].GetComponentInChildren<PlayerName>().AssignColor(playerList[i].playerColor, playerList[i].playerName);

            for (int w = 0; w < tempItemArray.Length; w++)
            {
                if (tempItemArray[w].GetComponent<Castle>())
                {
                    playerList[i].castleList.Add(tempItemArray[w].GetComponent<Castle>());
                    tempItemArray[w].GetComponent<Castle>().AssignCastle(playerList[i], castleIndex++);
                } else if(tempItemArray[w].CompareTag("CameraPosition"))
                {
                    playerList[i].cameraView = tempItemArray[w];
                } else if (tempItemArray[w].GetComponent<Army>())
                {
                    playerList[i].army = tempItemArray[w].GetComponent<Army>();
                    tempItemArray[w].GetComponent<Army>().fromPlayer = playerList[i].playerNumber;
                    break;
                } 
            }
        }
    }

    public void StartGame()
    {
        StartNextTurn();
        SetCameraToPlayer(currentPlayer.cameraView);
        UiManager.Instance.CloseAllPanels();
    }

    private void SetCameraToPlayer(Transform targetView)
    {
        AudioManager.Instance.Play(nextPlayerClip, transform, 0.2f);
        currentPlayer.playerNameUI.gameObject.SetActive(false);
        camera.enabled = true;
        camera.targetView = targetView;
    }

    private Player CalculateNextPlayer()
    {
        if(currentPlayer == null)
        {
            return playerList[Random.Range(0, playerList.Count)];
        }

        try
        {
            return playerList[playerList.IndexOf(currentPlayer) + 1];
        } catch (System.Exception)
        {
            return playerList[0];
        }
    }

    public int[] StartNextTurn(int cardDrawNumber = 1)
    {
        try
        {
            currentPlayer.playerNameUI.gameObject.SetActive(true);
        } catch (System.NullReferenceException)
        {
        }

        currentPlayer = (currentPlayer == nextPlayer || nextPlayer == null)? CalculateNextPlayer() : nextPlayer;
        nextPlayer = currentPlayer;
        UiManager.Instance.BroadCastMessage(currentPlayer.playerName + " may rule their realm", 3, currentPlayer.playerColor);
        SetCameraToPlayer(currentPlayer.cameraView);
        int[] tempArray = new int[] {currentPlayer.playerNumber, cardDrawNumber };
        return tempArray;
    }

    public void DestroyCastle(Castle castle)
    {
        AudioManager.Instance.Play(castleDestroyClip, transform, 0.3f);
        playerList[castle.fromPlayer].castleList.Remove(castle);
        StartCoroutine(castle.Implode());
        if(playerList[castle.fromPlayer].castleList.Count <= 0)
        {
            StartCoroutine(DestroyPlayer(playerList[castle.fromPlayer]));
        }
    }

    private IEnumerator DestroyPlayer(Player player)
    {
        yield return new WaitForSeconds(6);
        if (currentPlayer == player)
        {
            StartNextTurn();
        }
        Debug.Log(playerList.Count);
        playerList.Remove(player);
        Debug.Log(playerList.Count);
    }

    public void AssignCategory(int category)
    {
        AudioManager.Instance.Play(assignCategoryClip, transform, 0.1f);
        bool noAvailableCastle = true;
        for (int i = 0; i < currentPlayer.castleList.Count; i++)
        {
            if(currentPlayer.castleList[i].category == (Category.categories)4)
            {
                currentPlayer.castleList[i].ChangeCategory(category);
                noAvailableCastle = false;
                break;
            }
        }

        if (noAvailableCastle)
        {
            currentPlayer.castleList[0].ChangeCategory(category);
        }
    }

}
