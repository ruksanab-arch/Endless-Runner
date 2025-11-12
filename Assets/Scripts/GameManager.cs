using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Attempts Settings")]
    public int MaxAttempts = 3;
    public int RemainingAttempts;

    [Header("UI")]
    public TextMeshProUGUI AttemptsText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Persist across scenes
        RemainingAttempts = MaxAttempts;
        UpdateAttemptsUI();
    }

    public void OnPlayerDeath()
    {
        RemainingAttempts--;
        UpdateAttemptsUI();

        if (RemainingAttempts > 0)
        {
            Debug.Log("Attempts left: " + RemainingAttempts);
            // Restart level after 1 second
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("No attempts left! Game Over.");
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
    }

    private void UpdateAttemptsUI()
    {
        if (AttemptsText != null)
            AttemptsText.text = "Attempts: " + RemainingAttempts;
    }

    public void ResetAttempts()
    {
        RemainingAttempts = MaxAttempts;
        UpdateAttemptsUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}
