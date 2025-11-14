using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Gem Score Settings")]
    private int gemScore = 0;
    public TMP_Text scoreText;

    [Header("Attempts System")]
    public int maxAttempts = 3;
    public int currentAttempts = 3;

    public TMP_Text attemptText;
    public TMP_Text countdownText;

    public GameObject gameOverPanel;
    public GameObject noAttemptsPanel;
    public GameObject winPanel;

    public bool IsGameOver => isGameOver;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        // Load saved attempts
        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);

        if (currentAttempts < 0)
            currentAttempts = maxAttempts;

        UpdateScoreUI();
        UpdateAttemptUI();

        Time.timeScale = 1f;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit Obstacle!");
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

            // Increase gem count
            gemScore += 1;
            UpdateScoreUI();

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddGems(1);

            Debug.Log("Gem Collected! Total: " + gemScore);

            // ⭐ WIN CONDITION → 10 Gems
            if (gemScore >= 10)
            {
                WinGame();
            }
        }
    }

    // -----------------------------------------------------
    // ⭐ WIN GAME LOGIC ⭐
    // -----------------------------------------------------
    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;

        winPanel.SetActive(true);

        // Reward +20 Gems
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddGems(20);

        Debug.Log("WIN! +20 Gems");

        // Reset attempts on win
        PlayerPrefs.DeleteKey("AttemptsLeft");

        UpdateScoreUI();
        Time.timeScale = 0f; // Pause to show win panel
    }

    // -----------------------------------------------------
    // ⭐ GAME OVER LOGIC ⭐
    // -----------------------------------------------------
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Stop time
        Time.timeScale = 0f;

        // Deduct attempt
        currentAttempts--;
        if (currentAttempts < 0)
            currentAttempts = 0;

        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        if (currentAttempts > 0)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("Game Over! Attempts left: " + currentAttempts);
        }
        else
        {
            noAttemptsPanel.SetActive(true);
            Debug.Log("All attempts are over!");

            // ⭐ Lose penalty
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddGems(-10);

            Debug.Log("Penalty applied! -10 gems");

            StartCoroutine(ResetAttemptsAfterDelay(5f));
        }

        UpdateAttemptUI();
    }

    // -----------------------------------------------------
    // ⭐ RESET ATTEMPTS AFTER COOLDOWN ⭐
    // -----------------------------------------------------
    IEnumerator ResetAttemptsAfterDelay(float delay)
    {
        float remaining = delay;

        while (remaining > 0)
        {
            if (countdownText != null)
                countdownText.text = "Restarting in: " + Mathf.CeilToInt(remaining) + "s";

            yield return new WaitForSecondsRealtime(1f);
            remaining -= 1f;
        }

        currentAttempts = maxAttempts;
        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        countdownText.text = "Restarting...";
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // -----------------------------------------------------
    // ⭐ RESTART GAME BUTTON ⭐
    // -----------------------------------------------------
    public void RestartGameButton()
    {
        if (currentAttempts > 0)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            noAttemptsPanel.SetActive(true);
        }
    }

    // -----------------------------------------------------
    // ⭐ UI UPDATE ⭐
    // -----------------------------------------------------
    public void UpdateAttemptUI()
    {
        if (attemptText != null)
            attemptText.text = "Attempts: " + currentAttempts + " / " + maxAttempts;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Gems: " + gemScore;
    }
}
