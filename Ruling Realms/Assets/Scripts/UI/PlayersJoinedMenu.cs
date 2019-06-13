using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayersJoinedMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] playerNames;

    public void ShowJoinedPlayers()
    {
        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            playerNames[i].faceColor = GameManager.Instance.playerList[i].playerColor;
            playerNames[i].SetText(GameManager.Instance.playerList[i].playerName);

        }
    }

    public void StartCountDown()
    {
        gameObject.GetComponent<CountDownTimer>().enabled = true;
    }

}
