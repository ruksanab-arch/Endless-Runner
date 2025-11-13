using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DailyLoginManager : MonoBehaviour
{
    public TextMeshProUGUI rewardText;      // Text to show daily reward message
    public TextMeshProUGUI totalGemsText;   // Optional — shows total gems

    private int totalGems;
    private int currentDay;
    private DateTime lastLoginDate;

    void Start()
    {
        // Load saved data
        totalGems = PlayerPrefs.GetInt("Gems", 0);
        currentDay = PlayerPrefs.GetInt("LoginDay", 0);

        string savedDate = PlayerPrefs.GetString("LastLoginDate", "");
        if (!string.IsNullOrEmpty(savedDate))
        {
            lastLoginDate = DateTime.Parse(savedDate);
        }
        else
        {
            lastLoginDate = DateTime.MinValue;
        }

        CheckDailyLogin();
        UpdateUI();
    }

    void CheckDailyLogin()
    {
        DateTime today = DateTime.Now.Date;

        // First login ever
        if (lastLoginDate == DateTime.MinValue)
        {
            GiveReward();
            return;
        }

        // Already claimed today
        if (lastLoginDate.Date == today)
        {
            rewardText.text = "Already claimed today!";
            return;
        }

        // If it’s a new day → give reward
        if ((today - lastLoginDate.Date).TotalDays >= 1)
        {
            GiveReward();
        }
    }

    void GiveReward()
    {
        currentDay++;

        // Reward logic: 10, 20, 30 (from day 3 onwards, always 30)
        int reward = currentDay == 1 ? 10 :
                     currentDay == 2 ? 20 : 30;

        totalGems += reward;

        // Save progress
        PlayerPrefs.SetInt("Gems", totalGems);
        PlayerPrefs.SetInt("LoginDay", currentDay);
        PlayerPrefs.SetString("LastLoginDate", DateTime.Now.ToString());
        PlayerPrefs.Save();

        rewardText.text = $"Daily Reward: +{reward} gems (Day {currentDay})";
        Debug.Log($"Daily reward: {reward} gems | Day {currentDay}");
    }

    void UpdateUI()
    {
        if (totalGemsText != null)
        {
            totalGemsText.text = $"Gems: {totalGems}";
        }
    }
}
