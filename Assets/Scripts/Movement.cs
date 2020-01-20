using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private Transform camera;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction, int modifier)
    {
        direction = camera.TransformDirection(direction); // Face Camera
        direction.y = 0f; // prevent player from moving vertically
        direction.Normalize();
        rb.MovePosition(rb.position + (direction * speed * modifier) * Time.deltaTime);
        Rotate(direction);
    }
    public void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, rb.transform.up);
            Quaternion newRotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            rb.MoveRotation(newRotation);
        }
        else return; 
    }
}