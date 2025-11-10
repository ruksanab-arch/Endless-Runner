using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private const string GEMS_KEY = "PlayerGems";  // Renamed to match your purpose
    public int Gems { get; private set; }

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI GemText; // Drag UI text here in Inspector

    private void Awake()
    {
        // Singleton pattern (only one instance)
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

    public void AddGems(int amount)
    {
        Gems += amount;
        SaveGems();
        UpdateGemUI();
    }

    public void SaveGems()
    {
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
    }

    public void LoadGems()
    {
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }

    private void UpdateGemUI()
    {
        if (GemText != null)
            GemText.text = "Gems: " + Gems.ToString();
    }
}