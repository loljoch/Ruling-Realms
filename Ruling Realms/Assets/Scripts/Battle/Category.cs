using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    //names may change
    public enum categories
    {
        Cross,
        Crown,
        Castle,
        Book,
        None
    }

    public categories category = categories.None;
}
