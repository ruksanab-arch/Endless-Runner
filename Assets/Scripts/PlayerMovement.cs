using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for UI elements like score

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    public float speed = 5f;      // Left/right movement speed
    public float jumpForce = 7f;  // Jump strength
    public float minX = -2f;      // Left boundary
    public float maxX = 2f;       // Right boundary

    private bool isGrounded = true;

    // Score
    private int score = 0;
    public Text scoreText; // Assign a UI Text in Inspector

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        UpdateScoreUI();
    }

    void Update()
    {
        // LEFT/RIGHT movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newVelocity = new Vector3(horizontalInput * speed, playerRb.velocity.y, 0);
        playerRb.velocity = newVelocity;

        // Clamp player position within road boundaries
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        transform.position = clampedPos;

        // Running animation
        playerAnim.SetBool("isRunning", horizontalInput != 0);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnim.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle! Game Over!");
            // Add game over logic here
        }

        if (collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject); // Remove gem
            score += 1;                    // Increase score
            UpdateScoreUI();               // Update UI
            Debug.Log("Gem Collected! Score: " + score);
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}