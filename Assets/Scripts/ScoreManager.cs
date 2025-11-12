using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private const string GEMS_KEY = "PlayerGems";
    public int Gems { get; private set; }

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI GemText; // Assign in Inspector

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGems();
    }

    private void Start()
    {
        UpdateGemUI();
    }

    // 🪙 Add gems (called when player collects)
    public void AddGems(int amount)
    {
        Gems += amount;
        SaveGems();
        UpdateGemUI();
    }

    // 💰 Check if player has enough gems
    public bool HasEnoughGems(int amount)
    {
        return Gems >= amount;
    }

    // 🧾 Spend gems (for entry fee or shop)
    public bool SpendGems(int amount)
    {
        if (!HasEnoughGems(amount))
            return false;

        Gems -= amount;
        SaveGems();
        UpdateGemUI();
        return true;
    }

    // 🧩 Save & Load
    public void SaveGems()
    {
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
    }

    public void LoadGems()
    {
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }

    // 🔄 Update UI
    private void UpdateGemUI()
    {
        if (GemText != null)
            GemText.text = "Gems: " + Gems.ToString();
    }
}
