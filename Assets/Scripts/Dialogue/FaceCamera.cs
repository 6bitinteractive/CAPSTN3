using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Axis axis = Axis.Up;
    [SerializeField] private bool reverseFace = false;

    private static Camera referenceCamera;
    private Transform thisTransform;
    private bool active;

    private void Start()
    {
        referenceCamera = referenceCamera ?? SingletonManager.GetInstance<CameraManager>().MainCam;
        thisTransform = transform;
        active = true;
    }

    private void OnDisable()
    {
        referenceCamera = null;
        active = false;
    }

    private void LateUpdate()
    {
        if (!active)
            return;

        Vector3 targetPosition = thisTransform.position + (referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back));
        Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis);
        thisTransform.LookAt(targetPosition, targetOrientation);
    }

    private Vector3 GetAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.Up:
                return Vector3.up;
            case Axis.Down:
                return Vector3.down;
            case Axis.Left:
                return Vector3.left;
            case Axis.Right:
                return Vector3.right;
            case Axis.Forward:
                return Vector3.forward;
            case Axis.Back:
                return Vector3.back;
        }

        return Vector3.up;
    }

    public enum Axis
    {
        Up,
        Down,
        Left,
        Right,
        Forward,
        Back
    }
}

// Source: https://wiki.unity3d.com/index.php/CameraFacingBillboard
