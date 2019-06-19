using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public List<string> names;

    public void FakeJoinPlayer()
    {
        GameManager.Instance.AssignPlayer(new Vector3(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255)), names[Random.Range(0, names.Count)]);
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

    public void FakeFocusCastle()
    {
        BattleManager.Instance.GetCastlesFromPlayer(0);
        BattleManager.Instance.FocusCastle(0);
    }

    public void FakeAttack(int attackValue)
    {
        BattleManager.Instance.Attack(2, attackValue, 1);
    }

    public void FakeAttackTwo(int attackValue)
    {
        BattleManager.Instance.Attack(3, attackValue, 1);
    }

    public void FakeAttackThree(int attackValue)
    {
        BattleManager.Instance.Attack(1, attackValue, 1);
    }

    public void FakeDefend(int defendValue)
    {
        BattleManager.Instance.Defend(defendValue, 1);
    }

    public void FakeStartBattle()
    {
        BattleManager.Instance.StartBattle();
    }

    public void Automate()
    {
        FakeFocusCastle();
        FakeAttack(Random.Range(1, 13));
        FakeAttackTwo(Random.Range(1, 13));
        FakeAttackThree(Random.Range(1, 13));
        FakeDefend(Random.Range(1, 13));
        FakeStartBattle();
    }
}
