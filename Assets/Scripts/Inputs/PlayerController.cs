﻿using System;
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
    private Dig dig;
    private Talk talk;
    private Interactor interactor;
    private Vector3 currentMove;

    void Awake()
    {
        controlScheme = new PlayerControlScheme();
        playerInput = GetComponent<PlayerInput>();
        bark = GetComponent<Bark>();
        bite = GetComponent<Bite>();
        dig = GetComponent<Dig>();
        interactor = GetComponent<Interactor>();
        talk = GetComponent<Talk>();
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
            // Default movement
            if (controlScheme.Player.enabled)
            {
                movement.Move(currentMove, 1);
            }
            // If biting reverse controls
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
        // Makes sure the target exists and has the component biteable otherwise return
        if (interactor.CurrentTarget != null && !interactor.CurrentTarget.GetComponent<Biteable>()) return;

        // If the player is no longer biting
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

    public void Dig()
    {
        // Makes sure the target exists and has the component digable otherwise return
        if (interactor.CurrentTarget != null)
        {
            Digable digable = interactor.CurrentTarget.GetComponent<Digable>();
            DigableTerrain digableTerrain = interactor.CurrentTarget.GetComponent<DigableTerrain>();

            // Check if target is digable
            if (digable != null) dig.DigEvent(interactor, digable);

            // Check if target is a digable terrain
            if (digableTerrain != null) dig.DigTerrainEvent(interactor, digableTerrain);       
        }
    }

    public void Talk()
    {
        // Makes sure the target exists and has the component digable otherwise return
        if (interactor.CurrentTarget != null)
        {
            Talkable talkable = interactor.CurrentTarget.GetComponent<Talkable>();

            // Check if target is talkable
            if (talkable != null) talk.TalkEvent(interactor, talkable);
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
        controlScheme.Player.Dig.performed += context => Dig();
        controlScheme.Player.Talk.performed += context => Talk();
        controlScheme.Player.Move.performed += HandleMove;
        controlScheme.Player.Move.canceled += CancelMove;

        //Player Biting Control Scheme
        controlScheme.PlayerBiting.Bite.performed += context => Bite();
        controlScheme.PlayerBiting.Move.performed += HandleMove;
        controlScheme.PlayerBiting.Move.canceled += CancelMove; 
    }
}