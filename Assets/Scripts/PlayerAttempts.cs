using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAttempts : MonoBehaviour
{
    [Header("Attempts Settings")]
    public int maxAttempts = 3; // Total attempts
    private int currentAttempts;

    [Header("UI")]
    public TextMeshProUGUI attemptsText;  // Text to show attempts
    public GameObject gameOverPanel;       // Assign your GameOver panel here

    void Start()
    {
        currentAttempts = maxAttempts;     // Start with full attempts
        UpdateAttemptsUI();
        gameOverPanel.SetActive(false);    // Hide GameOver panel at start
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only count collisions with obstacles
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (currentAttempts > 0)
            {
                currentAttempts--;        // Reduce attempts by 1
                UpdateAttemptsUI();

                // If attempts reach 0, show Game Over
                if (currentAttempts == 0)
                {
                    gameOverPanel.SetActive(true);
                }
            }
        }
    }

    void UpdateAttemptsUI()
    {
        if (currentAttempts > 0)
            attemptsText.text = "Attempts: " + currentAttempts;
        else
            attemptsText.text = "No attempts left!";
    }
}
