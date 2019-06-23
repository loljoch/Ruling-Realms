using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlayers : MonoBehaviour
{
    [SerializeField] private List<InputPlayer> inputs;
    [SerializeField] private GameObject broadCastMessage;
    [SerializeField] private HandleInput inputHandler;

    public void AssignPlayers()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            GameManager.Instance.AssignPlayer(inputs[i].color, inputs[i].playerName);
        }

        broadCastMessage.SetActive(true);
        inputHandler.gameObject.SetActive(true);
        GameManager.Instance.AssignPlayer(inputs[0].color, inputs[0].playerName);
    }
}
