using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFight : MonoBehaviour
{
    public bool attackersWon;
    private int defendingArmyValue;
    private int attackingArmyValue;
    private ParticleSystem[] dustCloud;
    public AudioSource audioSource;

    private void Start()
    {
        dustCloud = GetComponentsInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
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
        List<Army> fightingArmies = BattleManager.Instance.GetFightingArmies();


        for (int i = 0; i < fightingArmies.Count; i++)
        {
            if (fightingArmies[i].activeArmy[0].attacking)
            {
                attackingArmyValue += fightingArmies[i].GetArmyStrength();
            } else if (!fightingArmies[i].activeArmy[0].attacking)
            {
                defendingArmyValue += fightingArmies[i].GetArmyStrength();
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
