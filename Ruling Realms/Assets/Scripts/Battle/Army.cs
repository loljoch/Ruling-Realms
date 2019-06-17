using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
    [SerializeField] private float marchTime;
    private int currentPositionTaken;
    public Animator animator;
    public List<AttackValue> jokers, bannerman, soldiers, thiefs, priests, ogres;
    public List<AttackValue> activeArmy;
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
                case -1:
                    jokers.Add(unit);
                    break;
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

    public void SetArmyActive(List<AttackValue> soldierSort, int amount, bool attacking)
    {
        for (int i = 0; i < amount; i++)
        {
            activeArmy.Add(soldierSort[currentPositionTaken]);
            soldierSort[currentPositionTaken].attacking = attacking;
            soldierSort[currentPositionTaken++].gameObject.SetActive(true);
        }
    }

    public void Reveal()
    {
        animator.SetBool("Revealed", true);
        StartCoroutine(SetRigidBodies(true));
    }


    public void SetBackToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    IEnumerator SetRigidBodies(bool active)
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < activeArmy.Count; i++)
        {
            activeArmy[i].GetComponent<Collider>().enabled = active;
            activeArmy[i].GetComponent<Rigidbody>().useGravity = active;
        }
    }

    public IEnumerator MarchArmy(Vector3 targetPosition)
    {
        Vector3 newTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        do
        {
            transform.position = Vector3.LerpUnclamped(transform.position, newTargetPosition, marchTime * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        } while (Vector3.Distance(transform.position, newTargetPosition) > 2);
        BattleManager.Instance.ArmyArrived();
    }

}
