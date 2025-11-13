using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpForce = 7f;
    public float minX = -2f;
    public float maxX = 2f;

    private bool isGrounded = true;

    // Internal game over flag
    public bool isGameOver = false;

    [Header("Score Settings")]
    private int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverPanel;

    // Public property to access isGameOver
    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        // Player horizontal movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newVelocity = new Vector3(horizontalInput * speed, playerRb.velocity.y, 0);
        playerRb.velocity = newVelocity;

        // Clamp position
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        transform.position = clampedPos;

        // Animation
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
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle! Game Over!");
            GameOver();
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            score += 1;
            UpdateScoreUI();

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddGems(1);

            Debug.Log("Gem Collected! Score: " + score);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    // Method to set game over
    public void GameOver()
{
    isGameOver = true;

    // Stop player movement
    playerRb.velocity = Vector3.zero;
    playerRb.isKinematic = true;

    // Show Game Over UI
    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);
}
}
