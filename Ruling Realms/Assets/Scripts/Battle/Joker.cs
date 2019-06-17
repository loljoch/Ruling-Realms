using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joker : AttackValue
{
    [SerializeField] private float growDivider;

    public IEnumerator GrowToValue(int value)
    {
        float tempValue = value * growDivider;
        Vector3 targetScale = transform.localScale * tempValue;

        do
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 2 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        } while (transform.localScale.x < tempValue - 0.2f);
    }
}
