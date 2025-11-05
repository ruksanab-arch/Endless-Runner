using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 8f; // how fast player moves forward
    public float sideSpeed = 5f; 

    private Rigidbody rb; // declare the Rigidbody variable here

    void Start()
    {
        // get Rigidbody component on this GameObject
        rb = GetComponent<Rigidbody>();

        // freeze rotations so player doesn't tip over
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        // move forward constantly along Z-axis
        Vector3 velocity = rb.velocity;
        velocity.z = forwardSpeed;
        rb.velocity = velocity;
    }
}