using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This just makes it easy for other scripts to get the main camera of the scene
public class CameraManager : Singleton<CameraManager>
{
    public Camera MainCam { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MainCam = GetComponent<Camera>();
    }
}
