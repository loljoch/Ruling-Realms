using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bannerman : AttackValue
{
    [SerializeField] private Renderer flag;
    [SerializeField] private Material[] bannerSymbols;

    public void ChangeToSymbol(int index)
    {
        Debug.Log(bannerSymbols.Length);
        flag.material = bannerSymbols[index];
    }


}
