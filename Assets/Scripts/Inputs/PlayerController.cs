using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControlScheme controlScheme;
    PlayerInput playerInput;
    private Movement movement;
    private Bark bark;
    private Bite bite;
    private Interactor interactor;
    private Vector3 currentMove;

    void Awake()
    {
        controlScheme = new PlayerControlScheme();
        playerInput = GetComponent<PlayerInput>();
        bark = GetComponent<Bark>();
        bite = GetComponent<Bite>();
        interactor = GetComponent<Interactor>();
        movement = GetComponent<Movement>();

        SetupPlayerConrtolScheme();    
    }

    void OnEnable()
    {
        controlScheme.Player.Enable();
    }

    void OnDisable()
    {
       controlScheme.Player.Disable();
       controlScheme.PlayerBiting.Disable();
    }

    private void FixedUpdate()
    {
        // If can move
        if (!movement.enabled) return;
        {
            if (controlScheme.Player.enabled)
            {
                movement.Move(currentMove, 1);
            }
            else if (controlScheme.PlayerBiting.enabled)
            {
                movement.Move(currentMove, -1);
            }
        }
    }

    public void Bark()
    {
         bark.BarkEvent(interactor);
    }

    public void Bite()
    {       
        // Makes sure there is always a target and that the player is no longer biting
        if (interactor.CurrentTarget != null && !bite.IsBiting)
        {
            // Turn off interaction with other objects
            interactor.CanInteract = false;

            // If player is not biting anything and the target is pullable
            if (!bite.IsBiting && interactor.CurrentTarget.GetComponent<Pullable>())
            {
                if (!controlScheme.PlayerBiting.enabled) SwitchToBitingControlScheme(); // Switch control scheme to reversed controls
                bite.BiteEvent(interactor, interactor.CurrentTarget.GetComponent<Biteable>());
                return;
            }

            // If player is not biting anything and the target is pullable
            else if (!bite.IsBiting)
            {
                bite.BiteEvent(interactor, interactor.CurrentTarget.GetComponent<Biteable>());
                return;
            }
        }

        // Check if bite target exists and if player is still biting
        if (interactor.CurrentTarget != null && bite.IsBiting)
        {
            bite.Release(interactor, interactor.CurrentTarget.GetComponent<Biteable>());
            SwitchToDefaultControlScheme(); // If no longer biting switch to default control scheme
            interactor.CurrentTarget = null;
            interactor.CanInteract = true;
            return;
        }
    }

    private void HandleMove(InputAction.CallbackContext context)
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

    public void SwitchToBitingControlScheme()
    {
        controlScheme.Player.Disable();
        controlScheme.PlayerBiting.Enable();
        playerInput.SwitchCurrentActionMap("PlayerBiting");
    }

    public void SwitchToDefaultControlScheme()
    {
        controlScheme.PlayerBiting.Disable();
        controlScheme.Player.Enable();       
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void SetupPlayerConrtolScheme()
    {
        //Default Player Control Scheme
        controlScheme.Player.Bark.performed += context => Bark();
        controlScheme.Player.Bite.performed += context => Bite();
        controlScheme.Player.Move.performed += HandleMove;
        controlScheme.Player.Move.canceled += CancelMove;

        //Player Biting Control Scheme
        controlScheme.PlayerBiting.Bite.performed += context => Bite();
        controlScheme.PlayerBiting.Move.performed += HandleMove;
        controlScheme.PlayerBiting.Move.canceled += CancelMove; 
    }
}