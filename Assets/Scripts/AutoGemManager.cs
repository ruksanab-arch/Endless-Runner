using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGemManager : MonoBehaviour
{
    public int AutoGemAmount = 30;          // gems per interval
    public float AutoGemInterval = 1800f;   // 30 minutes in seconds

    private float gemTimer = 0f;

    private void Update()
    {
        if (ScoreManager.Instance == null) return;

        // Only add auto gems if the game is running
        if (GameIsRunning())
        {
            gemTimer += Time.deltaTime;

            if (gemTimer >= AutoGemInterval)
            {
                ScoreManager.Instance.AddGems(AutoGemAmount);
                Debug.Log($"Auto gems added: +{AutoGemAmount}");
                gemTimer = 0f; // reset timer
            }
        }
    }

    private bool GameIsRunning()
    {
        // Replace with your actual game running check
        // Example: return !GameManager.Instance.isGameOver;
        return true;
    }
}