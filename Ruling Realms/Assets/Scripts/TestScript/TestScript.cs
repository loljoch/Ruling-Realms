using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public List<string> names;

    public void FakeJoinPlayer()
    {
        GameManager.Instance.AssignPlayer(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), names[Random.Range(0, names.Count)]);
    }

    public void FakeNextPlayerTurn()
    {
        GameManager.Instance.StartNextTurn();
    }

    public void FakeForcePlayerTurn()
    {
        GameManager.Instance.nextPlayer = GameManager.Instance.playerList[Random.Range(0, GameManager.Instance.playerList.Count)];
        GameManager.Instance.StartNextTurn();
    }
}
