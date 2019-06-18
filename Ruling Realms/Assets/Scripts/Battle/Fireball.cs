using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float fireballSpeed;
    private Transform fireballParent;
    public Army target;

    private void Start()
    {
        fireballParent = transform.parent;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.transform.GetChild(0).transform.position.x, target.transform.GetChild(0).transform.position.y - 6, target.transform.GetChild(0).transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, fireballSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.transform.GetChild(0).transform.position) < 3)
            {
                target.gameObject.SetActive(false);
            }
        }
    }

    public void ResetPositon()
    {
        transform.position = fireballParent.position;
        enabled = false;
    }
}
