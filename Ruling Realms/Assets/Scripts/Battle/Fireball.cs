using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float fireballSpeed;
    private Transform fireballParent;
    public Vector3 target;

    private void Start()
    {
        fireballParent = transform.parent;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.x, target.y - 6, target.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, fireballSpeed * Time.deltaTime);
        }
    }

    public void ResetPositon()
    {
        transform.position = fireballParent.position;
        enabled = false;
    }
}
