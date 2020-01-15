using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControlScheme controlScheme;
    private Movement movement;
    private Bark bark;
    private Bite bite;
    private Interactor interactor;
    private Vector3 currentMove;

    void Awake()
    {
        controlScheme = new PlayerControlScheme();
        bark = GetComponent<Bark>();
        bite = GetComponent<Bite>();
        interactor = GetComponent<Interactor>();
        movement = GetComponent<Movement>();

        controlScheme.Player.Bark.performed += context => Bark();
        controlScheme.Player.Bite.performed += context => Bite();
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
       bark.BarkEvent();
    }

    public void Bite()
    {
        if (interactor.CurrentTarget != null) bite.BiteEvent(interactor.CurrentTarget.GetComponent<Biteable>());
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