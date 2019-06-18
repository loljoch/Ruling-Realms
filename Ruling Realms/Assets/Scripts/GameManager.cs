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
    }

    public void AssignPlayer(Vector3 rgb, string playerName)
    {
        if (playerList.Count < maxPlayers)
        {
            Color32 tempPlayerColor = new Color32((byte)rgb.x, (byte)rgb.y, (byte)rgb.z, 255);
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

    private void AssignItemsToPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            int castleIndex = 0;
            Transform[] tempItemArray = playerItemList[i].GetComponentsInChildren<Transform>();
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
            return playerList[currentPlayer.playerNumber + 1];
        } catch (System.Exception)
        {
            return playerList[0];
        }
    }

    public int[] StartNextTurn(int cardDrawNumber = 1)
    {
        currentPlayer = (currentPlayer == nextPlayer || nextPlayer == null)? CalculateNextPlayer() : nextPlayer;
        nextPlayer = currentPlayer;
        UiManager.Instance.BroadCastMessage(currentPlayer.playerName + " may rule their realm", 3, currentPlayer.playerColor);
        SetCameraToPlayer(currentPlayer.cameraView);
        int[] tempArray = new int[] {currentPlayer.playerNumber, cardDrawNumber };
        return tempArray;
    }

    public void AssignCategory(int category)
    {
        //assigns castle category to a random current player
    }

}
