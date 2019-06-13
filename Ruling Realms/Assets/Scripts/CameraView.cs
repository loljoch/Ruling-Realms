using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public Transform targetView;

    [SerializeField] private float cameraLerpTime;

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, targetView.rotation, cameraLerpTime * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetView.position, cameraLerpTime * Time.deltaTime);
        if (transform.position == targetView.position)
        {
            Debug.Log("camera stopped lerping");
            enabled = false;
        }
    }

}
