﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleBanner : Bannerman
{
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
