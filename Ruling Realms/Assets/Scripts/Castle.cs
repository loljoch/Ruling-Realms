using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Category
{
    public int fromPlayer;
    public int castleIndex;
    [SerializeField] private float implodePower;

    public void AssignCastle(Player player, int castleIndex)
    {
        ColorBuildings(player.playerColor);
        fromPlayer = player.playerNumber;
        this.castleIndex = castleIndex;
    }

    private void ColorBuildings(Color32 color)
    {
        Renderer[] buildings = transform.parent.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < buildings.Length; i++)
        {
            for (int w = 0; w < buildings[i].materials.Length; w++)
            {
                if (buildings[i].materials[w].name == "PlayerCol (Instance)")
                {
                    buildings[i].materials[w].color = color;
                }
            }
        }
    }

    public IEnumerator Implode()
    {
        Rigidbody[] shards = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < shards.Length; i++)
        {
            shards[i].velocity = new Vector3(Random.Range(-3f, 3f), Random.Range(-4f, 4), Random.Range(-4f, 4f));
            shards[i].velocity *= implodePower;
        }

        yield return new WaitForSeconds(implodePower);
        for (int i = 0; i < shards.Length; i++)
        {
            shards[i].gameObject.SetActive(false);
        }
    }
}
