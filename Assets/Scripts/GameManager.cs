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
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        RemainingAttempts = MaxAttempts;
        SceneManager.sceneLoaded += OnSceneLoaded; // 👈 listen for scene reload
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find UI again after scene reload
        AttemptsText = GameObject.FindWithTag("AttemptsText")?.GetComponent<TextMeshProUGUI>();
        gameOverPanel = GameObject.FindWithTag("GameOverPanel");

        UpdateAttemptsUI();
    }

    public void OnPlayerDeath()
    {
        RemainingAttempts--;
        UpdateAttemptsUI();

        if (RemainingAttempts > 0)
        {
            Debug.Log("Attempts left: " + RemainingAttempts);
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
            AttemptsText.text = $"Attempts: {RemainingAttempts}/{MaxAttempts}";
    }

    public void ResetAttempts()
    {
        RemainingAttempts = MaxAttempts;
        UpdateAttemptsUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }
}