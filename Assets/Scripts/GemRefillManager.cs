using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemRefillManager : MonoBehaviour
{
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI timerText;

    private int gems;
    private const int refillAmount = 20;
    private const float refillInterval = 30f; // 30 minutes = 1800 seconds
    private float refillTimer = 0f;

    void Start()
    {
        // Load saved gems only (no real time)
        gems = PlayerPrefs.GetInt("Gems", 0);
        refillTimer = 0f;
        UpdateUI();
    }

    void Update()
    {
        refillTimer += Time.deltaTime; // counts only while game is open

        if (refillTimer >= refillInterval)
        {
            AddGems(refillAmount);
            refillTimer = 0f; // reset timer
        }

        UpdateTimerUI();
    }

    void AddGems(int amount)
    {
        gems += amount;
        PlayerPrefs.SetInt("Gems", gems);
        PlayerPrefs.Save();
        UpdateUI();
        Debug.Log($"+{amount} gems added! Total gems = {gems}");
    }

    void UpdateUI()
    {
        if (gemText != null)
            gemText.text = "Gems: " + gems;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float remaining = Mathf.Max(refillInterval - refillTimer, 0f);
            int minutes = Mathf.FloorToInt(remaining / 60);
            int seconds = Mathf.FloorToInt(remaining % 60);
            timerText.text = $"Earning gems in: {minutes:00}:{seconds:00}";
        }
    }
}
