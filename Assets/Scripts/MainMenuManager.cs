using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    [Header("Entry Fee Settings")]
    [SerializeField] private int entryFee = 20;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI messageText; // Optional: show warning message

    public void OnPlayButton()
    {
        // Ensure ScoreManager exists
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager not found in scene!");
            return;
        }

        // Check if player has enough gems
        if (ScoreManager.Instance.HasEnoughGems(entryFee))
        {
            // Deduct entry fee
            ScoreManager.Instance.SpendGems(entryFee);

            // Load gameplay scene
            SceneManager.LoadScene(gameplaySceneName);
        }
        else
        {
            // Not enough gems
            if (messageText != null)
            {
                messageText.text = "Not enough gems to play! (Need 20)";
                messageText.color = Color.red;
            }
            else
            {
                Debug.Log("Not enough gems to play! (Need 20)");
            }
        }
    }
}