using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public void AssignColor(Color32 color, string name)
    {
        GetComponent<TextMeshPro>().color = color;
        GetComponent<TextMeshPro>().SetText(name);
    }

    public void RotateToCamera()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(0, 180, 0);
    }

    private void FixedUpdate()
    {
        RotateToCamera();
    }
}
