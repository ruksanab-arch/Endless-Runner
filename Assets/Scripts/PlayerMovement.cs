using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 8f;
    public float sideSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        // Forward constant movement
        Vector3 velocity = rb.velocity;
        velocity.z = forwardSpeed;

        // Left-right movement
        float horizontalInput = Input.GetAxis("Horizontal");
        velocity.x = horizontalInput * sideSpeed;

        rb.velocity = velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        // If we collide with anything tagged "Ground", we can jump again
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
