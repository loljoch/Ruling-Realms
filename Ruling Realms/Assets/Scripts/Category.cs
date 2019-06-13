using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Category : MonoBehaviour
{
    //names may change
    public enum categories
    {
        None,
        One,
        Two,
        Three,
        Four
    }

    public categories category = categories.None;
}
