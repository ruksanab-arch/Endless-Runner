using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;

    public float speed = 5f;
    public float jumpForce = 7f;
    public float minX = -2f;
    public float maxX = 2f;

    private bool isGrounded = true;
    public bool isGameOver = false;

    // Score
    private int score = 0;
    public TMP_Text scoreText;
    public GameObject gameOverPanel;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        UpdateScoreUI();
    }

    void Update()
    {
        if (isGameOver) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newVelocity = new Vector3(horizontalInput * speed, playerRb.velocity.y, 0);
        playerRb.velocity = newVelocity;

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        transform.position = clampedPos;

        playerAnim.SetBool("isRunning", horizontalInput != 0);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            playerAnim.SetTrigger("Jump");
        }
    }

    // ✅ Keep this for physics collisions (ground, obstacle)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle! Game Over!");
            GameOver();
        }
    }

    // ✅ NEW: Use Trigger for Gems (instead of collision)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            score += 1;
            UpdateScoreUI();
            Debug.Log("Gem Collected! Score: " + score);
        }
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddGems(1);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        playerRb.velocity = Vector3.zero;
        playerRb.isKinematic = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Invoke(nameof(RestartGame), 2f);
    }

    void RestartGame()
    {
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