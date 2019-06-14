using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    public List<AttackValue> bannerman, soldiers, thiefs, priests, ogres;
    private int currentPositionTaken;
    private Animator animator;
    public Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
        animator = GetComponent<Animator>();

        currentPositionTaken = 0;

        foreach (var unit in GetComponentsInChildren<AttackValue>())
        {
            switch (unit.attackValue)
            {
                case 0:
                    bannerman.Add(unit);
                    break;
                case 1:
                    soldiers.Add(unit);
                    break;
                case 2:
                    thiefs.Add(unit);
                    break;
                case 3:
                    priests.Add(unit);
                    break;
                case 10:
                    ogres.Add(unit);
                    break;
                default:
                    break;
            }

            unit.gameObject.SetActive(false);
        }
    }

    public void SetArmyActive(List<AttackValue> soldierSort ,int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            soldierSort[currentPositionTaken++].gameObject.SetActive(true);
        }
    }

    public void Reveal()
    {
        animator.SetBool("Revealed", true);
    }


    public void SetBackToOriginalPosition()
    {
        transform.position = originalPosition;
    }

}
