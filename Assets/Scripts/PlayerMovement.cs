using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    public bool isGameOver = false;

    [Header("Gem Score Settings")]
    private int gemScore = 0;
    public TMP_Text scoreText;

    [Header("Attempts System")]
    public int maxAttempts = 3;
    public int currentAttempts = 3;

    [Header("Hearts UI")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    public TMP_Text countdownText;
    public GameObject gameOverPanel;
    public GameObject noAttemptsPanel;
    public GameObject winPanel;

    [Header("Gem UI Animator")]
    public GemUIAnimator gemAnimator;

    private bool firstHitIgnored = false;

    public bool IsGameOver => isGameOver;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();

        currentAttempts = PlayerPrefs.GetInt("AttemptsLeft", maxAttempts);
        if (currentAttempts < 0) currentAttempts = maxAttempts;

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
        {
            isGrounded = true;
            return;
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gem"))
        {
            Destroy(other.gameObject);
            gemScore++;
            UpdateScoreUI();

            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddGems(1);

            if (gemAnimator != null)
                gemAnimator.PlayGemAnimation();

            if (gemScore >= 10)
                WinGame();
        }
    }

    void WinGame()
    {
        if (isGameOver) return;
        isGameOver = true;
        winPanel.SetActive(true);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddGems(20);

        PlayerPrefs.DeleteKey("AttemptsLeft");
        UpdateScoreUI();
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        if (!firstHitIgnored)
        {
            firstHitIgnored = true;
            return;
        }

        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;

        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.isKinematic = true;
        }

        currentAttempts = Mathf.Max(0, currentAttempts - 1);
        PlayerPrefs.SetInt("AttemptsLeft", currentAttempts);
        PlayerPrefs.Save();

        UpdateAttemptUI();

        if (currentAttempts > 0)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            noAttemptsPanel.SetActive(true);
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddGems(-10);

            StartCoroutine(ResetAttemptsAfterDelay(5f));
        }
    }

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

        if (countdownText != null)
            countdownText.text = "Restarting...";

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RetryButton()
    {
        if (currentAttempts > 0)
        {
            if (playerRb != null)
                playerRb.isKinematic = false;

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            noAttemptsPanel.SetActive(true);
        }
    }

    void UpdateAttemptUI()
    {
        AnimateHeart(heart1, currentAttempts >= 1);
        AnimateHeart(heart2, currentAttempts >= 2);
        AnimateHeart(heart3, currentAttempts >= 3);
    }

    void AnimateHeart(Image heart, bool show)
    {
        if (heart == null) return;

        if (show)
        {
            heart.gameObject.SetActive(true);
            heart.rectTransform.localScale = Vector3.one;
        }
        else
        {
            StartCoroutine(FallHeart(heart));
        }
    }

    IEnumerator FallHeart(Image heart)
    {
        Vector3 originalPos = heart.rectTransform.localPosition;
        Vector3 targetPos = originalPos + new Vector3(0, -50f, 0);
        float timer = 0f;
        float duration = 0.3f;

        while (timer < duration)
        {
            heart.rectTransform.localPosition = Vector3.Lerp(originalPos, targetPos, timer / duration);
            heart.rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / duration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        heart.rectTransform.localPosition = originalPos;
        heart.rectTransform.localScale = Vector3.one;
        heart.gameObject.SetActive(false);
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Gems: " + gemScore;
    }
}
