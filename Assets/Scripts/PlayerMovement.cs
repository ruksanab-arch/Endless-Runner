using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 8f;   // Forward constant speed
    public float sideSpeed = 5f;      // Left-right movement speed
    public float jumpForce = 7f;      // Jump power

    [Header("Ground Check")]
    public Transform groundCheck;     // Empty object under player
    public float groundDistance = 0.3f;
    public LayerMask groundLayer;     // Assign your "Ground" layer here

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // Check if player is on ground (raycast down)
        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundLayer);

        // Jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
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

    private void OnDrawGizmosSelected()
    {
        // visualize ground check ray in Scene view
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundDistance);
        }
    }
}