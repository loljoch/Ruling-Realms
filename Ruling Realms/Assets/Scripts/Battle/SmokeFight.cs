using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFight : MonoBehaviour
{
    private int defendingArmyValue;
    private int attackingArmyValue;
    private List<AttackValue> attackingCollidedUnits;
    private List<AttackValue> defendingCollidedUnits;
    private GameObject dustCloud;

    private void Start()
    {
        attackingCollidedUnits = new List<AttackValue>();
        defendingCollidedUnits = new List<AttackValue>();
        dustCloud = GetComponentsInChildren<Transform>()[1].gameObject;
        dustCloud.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {
            dustCloud.SetActive(true);
            AttackValue currentCollision = other.gameObject.GetComponent<AttackValue>();

            if (currentCollision.attacking)
            {
                attackingArmyValue += currentCollision.attackValue;
                attackingCollidedUnits.Add(currentCollision);
            } else if (!currentCollision.attacking)
            {
                defendingArmyValue += currentCollision.attackValue;
                defendingCollidedUnits.Add(currentCollision);
            }
            currentCollision.gameObject.SetActive(CheckIfDisable(currentCollision.attacking));
        } catch (System.NullReferenceException)
        {
        }
    }

    private bool CheckIfDisable(bool attacking)
    {
        if (defendingArmyValue != 0 || attackingArmyValue != 0)
        {
            switch (attacking)
            {
                case true:
                    if (attackingArmyValue > defendingArmyValue)
                    {
                        return false;
                    } else
                    {
                        return true;
                    }
                case false:
                    if (defendingArmyValue >= attackingArmyValue)
                    {
                        return false;
                    } else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        } else
        {
            return false;
        }
        
    }

    public bool HasTheDefenderWon()
    {
        if(defendingArmyValue >= attackingArmyValue)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
