﻿using System.Collections;
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

    public void AssignPlayer(float r, float g, float b, string playerName)
    {
        if (playerList.Count < maxPlayers)
        {
            Color32 tempPlayerColor = new Color32((byte)r, (byte)g, (byte)b, 255);
            playerList.Add(new Player(tempPlayerColor, playerName, playerList.Count));

            if (UiManager.Instance.playersJoinedMenu.gameObject.activeSelf)
            {
                UiManager.Instance.playersJoinedMenu.ShowJoinedPlayers();
            }
        } else
        {
            UiManager.Instance.playersJoinedMenu.StartCountDown();
            AssignCastlesToPlayers();
        }

    }

    private void AssignCastlesToPlayers()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            Transform[] tempItemArray = playerItemList[i].GetComponentsInChildren<Transform>();
            for (int w = 0; w < tempItemArray.Length; w++)
            {
                if (tempItemArray[w].GetComponent<Castle>())
                {
                    playerList[i].castleList.Add(tempItemArray[w].GetComponent<Castle>());
                    tempItemArray[w].GetComponent<Castle>().AssignCastle(playerList[i]);
                } else
                {
                    playerList[i].cameraView = tempItemArray[w];
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

    public void StartNextTurn()
    {

        currentPlayer = (currentPlayer == nextPlayer || nextPlayer == null)? CalculateNextPlayer() : nextPlayer;
        nextPlayer = currentPlayer;
        UiManager.Instance.BroadCastMessage(currentPlayer.playerName + " may rule their realm", 3, currentPlayer.playerColor);
        SetCameraToPlayer(currentPlayer.cameraView);
    }

}