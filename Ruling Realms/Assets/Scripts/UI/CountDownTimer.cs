using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime;

    private void FixedUpdate()
    {
        if (TimeLeft() > 0)
        {
            timerText.SetText("Match starts in " + TimeLeft() + "...");
        } else
        {
            GameManager.Instance.StartGame();
            enabled = false;
        }
    }

    private float TimeLeft()
    {
        currentTime += Time.deltaTime * 0.7f;
        return Mathf.Round(GameManager.startGameWaitingTime - currentTime);
    }
}
