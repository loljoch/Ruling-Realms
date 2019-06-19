using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private void Explode()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public void DisableSelf()
    {
        GetComponent<Animator>().SetBool("Shoot", false);
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void SetColor(Color32 playerColor)
    {
        GetComponent<TrailRenderer>().startColor = playerColor;
        ParticleSystem.MainModule particleSystem = GetComponentInChildren<ParticleSystem>().main;
        particleSystem.startColor = GetComponent<TrailRenderer>().startColor;
    }
}
