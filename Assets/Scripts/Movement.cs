using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Multipliers")]
    [SerializeField] private float fullMultiplier = 1.2f;
    [SerializeField] private float normalMultiplier = 1f;
    [SerializeField] private float hungryMultiplier = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private float gravity = -30f;
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float groundDistance = 0.4f;

    [Header("Other Fields")]
    [SerializeField] private Transform camera;
    [SerializeField] private Animator animator;

    private float currentMultiplier = 1f;
    private Vector3 currentDirection;
    private bool isGrounded;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Move(Vector3 direction, int modifier)
    {
        currentDirection = direction;
       
        CheckIfGrounded();
        GetDirection();
        Rotate(currentDirection);

        currentDirection.y += gravity * Time.deltaTime;
        controller.Move((currentDirection * speed * currentMultiplier * modifier) * Time.deltaTime);
      
        if (animator == null) return;
        Animate(currentDirection);
    }

    public void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Vector3 NextDir = new Vector3(direction.x, 0, direction.z);
            transform.rotation = Quaternion.LookRotation(NextDir * rotationSpeed);
        }
    }

    public void Animate(Vector3 direction)
    {
        animator.SetFloat("VelX", direction.x);
      //  animator.SetFloat("VelY", direction.y);
    }

    public void UpdateMultiplier(Hunger hunger)
    {
        switch (hunger.CurrentState)
        {
            case Hunger.State.Full:
                currentMultiplier = fullMultiplier;
                break;
            case Hunger.State.Normal:
                currentMultiplier = normalMultiplier;
                break;
            case Hunger.State.Hungry:
                currentMultiplier = hungryMultiplier;
                break;
        }
    }

    private void GetDirection()
    {
        //Get Direction
        currentDirection = camera.TransformDirection(currentDirection); // Face Camera
        currentDirection.y = 0f;
        currentDirection.Normalize();
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheckPos.position, groundDistance, layerMask);

        if (isGrounded && currentDirection.y < 0)
        {
            currentDirection.y = -2f;
            Debug.Log("grounded");
        }
    }
}