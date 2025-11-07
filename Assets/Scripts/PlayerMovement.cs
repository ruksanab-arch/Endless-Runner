using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // For restarting or loading GameOver scene

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    public float speed = 5f;
    public float jumpForce = 7f;
    public float minX = -2f;
    public float maxX = 2f;

    private bool isGrounded = true;
    public bool isGameOver = false; // ✅ added

    // Score
    private int score = 0;
    public Text scoreText;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        UpdateScoreUI();
    }

    void Update()
    {
        // ✅ Stop all movement when Game Over
        if (isGameOver) return;

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
            GameOver(); // ✅ call game over function
        }

        if (collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            score += 1;
            UpdateScoreUI();
            Debug.Log("Gem Collected! Score: " + score);
        }
    }

    public void GameOver()
    {
        isGameOver = true; // ✅ stop update logic
        playerRb.velocity = Vector3.zero;
        playerRb.isKinematic = true; // ✅ freeze physics
        playerAnim.SetTrigger("Death"); // ✅ if you have a “Death” animation

        // Optionally restart game after 2 seconds
        Invoke(nameof(RestartGame), 2f);
    }

    void RestartGame()
    {
        // reload current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}