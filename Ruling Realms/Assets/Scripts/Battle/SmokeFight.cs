using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFight : MonoBehaviour
{
    public bool attackersWon;
    private int defendingArmyValue;
    private int attackingArmyValue;
    private ParticleSystem[] dustCloud;

    private void Start()
    {
        dustCloud = GetComponentsInChildren<ParticleSystem>();
        SetDustcloud(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            SetDustcloud(true);
            AttackValue currentCollision = other.gameObject.GetComponent<AttackValue>();
            if (currentCollision.attacking)
            {
                attackingArmyValue -= currentCollision.attackValue != 0 ? currentCollision.attackValue : 1;
                if (!attackersWon)
                {
                    currentCollision.gameObject.SetActive(false);
                }
            } else
            {
                defendingArmyValue -= currentCollision.attackValue != 0 ? currentCollision.attackValue : 1;
                if (attackersWon)
                {
                    currentCollision.gameObject.SetActive(false);
                }
            }
            CheckIfBattleIsOver();

        } catch (System.NullReferenceException)
        {
        }
    }

    public void CalculateWinner()
    {
        List<List<AttackValue>> activeArmies = new List<List<AttackValue>>();

        for (int i = 0; i < BattleManager.Instance.GetFightingArmies().Count; i++)
        {
            if(GameManager.Instance.playerList[i].army.activeArmy.Count > 0)
            {
                activeArmies.Add(BattleManager.Instance.GetFightingArmies()[i].activeArmy);
            }
        }

        for (int i = 0; i < activeArmies.Count; i++)
        {
            for (int w = 0; w < activeArmies[i].Count; w++)
            {
                if (activeArmies[i][w].attacking)
                {
                    attackingArmyValue += activeArmies[i][w].attackValue;
                } else
                {
                    defendingArmyValue += activeArmies[i][w].attackValue;
                }

            }
        }
        attackersWon = defendingArmyValue >= attackingArmyValue ? false : true;
    }

    private void CheckIfBattleIsOver()
    {
        if(defendingArmyValue <= 0 || attackingArmyValue <= 0)
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(BattleManager.Instance.BattleEnd(attackersWon));
        }
    }

    public void SetDustcloud(bool active)
    {
        for (int i = 0; i < dustCloud.Length; i++)
        {
            if (active)
            {
                dustCloud[i].Play();
            } else
            {
                dustCloud[i].Stop();
            }
        }
    }

    public void ResetValues()
    {
        GetComponent<Collider>().enabled = true;
        attackingArmyValue = 0;
        defendingArmyValue = 0;
    }
}
