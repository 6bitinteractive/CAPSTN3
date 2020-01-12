using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControlScheme controlScheme;
    private Movement movement;
    private Vector3 currentMove;

    void Awake()
    {
        controlScheme = new PlayerControlScheme();
        movement = GetComponent<Movement>();
        controlScheme.Player.Bark.performed += context => Bark();
        controlScheme.Player.Move.performed += HandleMove;
        controlScheme.Player.Move.canceled += CancelMove;
    }

    void OnEnable()
    {
        controlScheme.Player.Enable();
    }

    void OnDisable()
    {
        controlScheme.Player.Disable();
    }

    private void FixedUpdate()
    {
        movement.Move(currentMove);
    }

    public void Bark()
    {
        // Replace with proper script later. currently only being used for testing the new Input Unity System
        Debug.Log("woof");
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        currentMove = context.ReadValue<Vector2>();
        // Play Moving Animation
        //Debug.Log(currentMove);  
    }
    private void CancelMove(InputAction.CallbackContext context)
    {
        currentMove = Vector2.zero;
        //Play Idle Animation
        return;
    }
}