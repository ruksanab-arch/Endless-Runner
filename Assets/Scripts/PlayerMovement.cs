using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 8f;   // forward movement speed
    public float sideSpeed = 5f;      // left-right movement speed
    public float jumpForce = 7f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // Forward movement (constant)
        Vector3 velocity = rb.velocity;
        velocity.z = forwardSpeed;

        // Left and right movement using input keys (A/D or Left/Right)
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 (left) to +1 (right)
        velocity.x = horizontalInput * sideSpeed;

        // Apply the new velocity
        rb.velocity = velocity;
    }
}