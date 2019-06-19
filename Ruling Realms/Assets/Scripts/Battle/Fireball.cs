using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float fireballSpeed;
    private Transform fireballParent;
    private Vector3 targetPosition;
    public Army target;

    private void Start()
    {
        fireballParent = transform.parent;
    }

    private void OnEnable()
    {
        try
        {
            targetPosition = new Vector3(target.transform.GetChild(0).transform.position.x, target.transform.GetChild(0).transform.position.y - 6, target.transform.GetChild(0).transform.position.z);
        } catch (System.NullReferenceException)
        {
        }
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, fireballSpeed * Time.deltaTime);
            CheckDistance();
        }
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(transform.position, target.transform.GetChild(0).transform.position) < 3)
        {
            for (int i = 0; i < target.activeArmy.Count; i++)
            {
                target.activeArmy[i].gameObject.SetActive(false);
            }
            target.activeArmy.Clear();
            StartCoroutine(target.Reset());
        }
    }

    public void ResetPositon()
    {
        transform.position = fireballParent.position;
        enabled = false;
    }
}
