using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkMachine : MonoBehaviour
{

    [SerializeField] private List<Firework> fireworks;

    public IEnumerator FireFireworks(List<Color32> playerColors)
    {
        int index = 0;
        for (int i = 0; i < playerColors.Count; i++)
        {
            ShootFirework(index++, playerColors[Random.Range(0, playerColors.Count)]);
            ShootFirework(index++, playerColors[Random.Range(0, playerColors.Count)]);
            ShootFirework(index++, playerColors[Random.Range(0, playerColors.Count)]);

            yield return new WaitForSeconds(0.5f);
            

        }
        
    }

    private void ShootFirework(int index, Color32 playerColor)
    {
        fireworks[index].transform.Rotate(Vector3.up, Random.Range(-180, 180));
        fireworks[index].gameObject.SetActive(true);
        fireworks[index].SetColor(playerColor);
        fireworks[index].GetComponent<Animator>().SetTrigger("Shoot");
    }
}
