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
    private Dig dig;
    private Talk talk;
    private Sniff sniff;
    private Interactor interactor;
    private Vector3 currentMove;

    private SceneController sceneController;
    private EventManager eventManager;
    private DialogueDisplayManager dialogueDisplayManager;

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
        sniff = GetComponent<Sniff>();

        SetupPlayerConrtolScheme();
    }

    void OnEnable()
    {
        controlScheme.Player.Enable();

        sceneController = SingletonManager.GetInstance<SceneController>();
        sceneController.BeforePreviousSceneUnload.AddListener(() => enabled = false);
        sceneController.AfterCurrentSceneLoad.AddListener(() => enabled = true);

        eventManager = SingletonManager.GetInstance<EventManager>();
        eventManager.Subscribe<CutsceneEvent, Cutscene>(SwitchCutsceneControlScheme);

        // Fix: need to set up the proper settings (currently using arbitrary keys)
        dialogueDisplayManager = SingletonManager.GetInstance<DialogueDisplayManager>();
        dialogueDisplayManager.OnConversationBegin.AddListener(SwitchToDialogueInteractionControlScheme);
        dialogueDisplayManager.OnConversationEnd.AddListener(SwitchToDefaultControlScheme);
    }

    void OnDisable()
    {
        controlScheme.Player.Disable();
        controlScheme.PlayerBiting.Disable();

        // FIX-LowPriority?: While digging for the toy, PlayerController gets disabled which disables this which then becomes a blocker
        // because you can't continue the conversation
        // FIND the cause of what disables this (PlayerController) component during Dig interaction; it's is not among the SwitchX methods :(
        // On the other hand, it may not be really necessary to disable the dialogue control scheme whenever the PlayerController is disabled---for now???
        //controlScheme.DialogueInteraction.Disable();

        // Remove listeners
        eventManager.Unsubscribe<CutsceneEvent, Cutscene>(SwitchCutsceneControlScheme);
        dialogueDisplayManager.OnConversationBegin.RemoveListener(SwitchToDialogueInteractionControlScheme);
        dialogueDisplayManager.OnConversationEnd.RemoveListener(SwitchToDefaultControlScheme);
    }

    private void FixedUpdate()
    {
        Move();
    }

    #region Movement & Interactions
    public void Move()
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
        if (!bark.enabled) { return; } // Make sure it's not called when its corresponding components are disabled
        bark.BarkEvent(interactor);
    }

    public void Bite()
    {
        if (!bite.enabled) { return; }

        // Makes sure the target exists and has the component biteable otherwise return
        if (interactor.CurrentTarget == null) return;

        Biteable biteable = interactor.CurrentTarget.GetComponent<Biteable>();
        if (biteable == null) return;

        // If the player is no longer biting
        if (!bite.IsBiting)
        {
            // Turn off interaction with other objects
            interactor.CanInteract = false;

            // If player is not biting anything and the target is pullable
            Pullable pullable = interactor.CurrentTarget.GetComponent<Pullable>();
            if (pullable)
            {
                if (pullable.enabled)
                {
                    if (!controlScheme.PlayerBiting.enabled) SwitchToBitingControlScheme(); // Switch control scheme to reversed controls
                    bite.BiteEvent(interactor, biteable);
                    return;
                }
                else // Pullable component exists but is disabled
                {
                    interactor.CurrentTarget = null;
                    interactor.CanInteract = true;
                    return;
                }
            }

            // If player is not biting anything and the target is pickupable
            Pickupable pickupable = interactor.CurrentTarget.GetComponent<Pickupable>();
            if (pickupable)
            {
                if (pickupable.enabled)
                {
                    bite.BiteEvent(interactor, biteable);
                    return;
                }
                else // Pickupable component exists but is disabled
                {
                    interactor.CurrentTarget = null;
                    interactor.CanInteract = true;
                    return;
                }
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
        if (!dig.enabled) { return; }

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
        if (!talk.enabled) { return; }

        // Makes sure the target exists and has the component digable otherwise return
        if (interactor.CurrentTarget != null)
        {
            Talkable talkable = interactor.CurrentTarget.GetComponent<Talkable>();

            // Check if target is talkable
            if (talkable != null) talk.TalkEvent(interactor, talkable);
        }
    }

    public void ActivateScentMode()
    {
        sniff.ActivateScentMode();
    }

    public void DeactivateScentMode(InputAction.CallbackContext context)
    {
        sniff.DeactivateScentMode();
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
    #endregion

    public void SwitchToBitingControlScheme()
    {
        controlScheme.DialogueInteraction.Disable();
        controlScheme.Player.Disable();
        controlScheme.PlayerBiting.Enable();
        playerInput.SwitchCurrentActionMap("PlayerBiting");
    }

    public void SwitchToDefaultControlScheme()
    {
        // We don't allow a conversation from a cutscene to turn the controller scheme back to default
        if (playingCutscene)
            return;

        movement.enabled = true;
        controlScheme.DialogueInteraction.Disable();
        controlScheme.PlayerBiting.Disable();
        controlScheme.Player.Enable();
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void SwitchToDialogueInteractionControlScheme()
    {
        movement.Move(Vector3.zero, 0);
        movement.enabled = false;

        controlScheme.Player.Disable();
        controlScheme.PlayerBiting.Disable();
        controlScheme.DialogueInteraction.Enable();
        playerInput.SwitchCurrentActionMap("DialogueInteraction");
    }

    private bool playingCutscene;
    private void SwitchCutsceneControlScheme(Cutscene cutscene)
    {
        playingCutscene = true;

        switch (cutscene.CurrentState)
        {
            case Cutscene.State.Stopped:
                {
                    playingCutscene = false;
                    SwitchToDefaultControlScheme();
                    break;
                }

            case Cutscene.State.Playing:
            case Cutscene.State.Paused:
                {
                    SwitchToDialogueInteractionControlScheme();
                    break;
                }
        }
    }

    private void SetupPlayerConrtolScheme()
    {
        //Default Player Control Scheme
        controlScheme.Player.Bark.performed += context => Bark();
        controlScheme.Player.Bite.performed += context => Bite();
        controlScheme.Player.Dig.performed += context => Dig();
        controlScheme.Player.Talk.performed += context => Talk();
        controlScheme.Player.ScentMode.started += context => ActivateScentMode();
        controlScheme.Player.ScentMode.canceled += DeactivateScentMode;
        controlScheme.Player.Move.performed += HandleMove;
        controlScheme.Player.Move.canceled += CancelMove;

        //Player Biting Control Scheme
        controlScheme.PlayerBiting.Bite.performed += context => Bite();
        controlScheme.PlayerBiting.Move.performed += HandleMove;
        controlScheme.PlayerBiting.Move.canceled += CancelMove;
        controlScheme.PlayerBiting.Talk.performed += context => Talk();

        // Dialogue Interaction Control Scheme
        controlScheme.DialogueInteraction.Confirm.performed += context => dialogueDisplayManager.ContinueConversation();
        controlScheme.DialogueInteraction.Skip.performed += context => dialogueDisplayManager.EndConversation();
    }
}