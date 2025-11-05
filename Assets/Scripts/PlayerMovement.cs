using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim; // 🆕 Added for animations

    public float forwardSpeed = 8f;
    public float sideSpeed = 5f;
    public float jumpForce = 6f;

    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>(); // 🆕 Get the Animator

        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // 🆕 Play running animation at start
        anim.Play("Run_Static");  // use exact name from your animation clip
    }

    void Update()
    {
        // Move left/right
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput * sideSpeed, rb.velocity.y, forwardSpeed);
        rb.velocity = move;

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // 🆕 Play jump animation
            anim.Play("Jumping"); // change to your jump clip name
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            // 🆕 Return to running animation
            anim.Play("Run_Static"); // use your running clip name
        }
    }
}