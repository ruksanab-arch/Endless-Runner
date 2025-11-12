using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // Player data keys
    private const string GEMS_KEY = "PlayerGems";
    private const string SCORE_KEY = "TotalScore";
    private const string FIRST_ENTRY_KEY = "FirstEntry";

    public int Gems { get; private set; }
    public int TotalScore { get; private set; }

    [Header("UI (Optional, auto-detected if null)")]
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI ScoreText;

    [Header("Free Entry Settings")]
    public int FreeGems = 50; // gems given on first entry

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved data
        LoadGems();
        LoadScore();

        // Give free gems on first entry
        if (!PlayerPrefs.HasKey(FIRST_ENTRY_KEY))
        {
            Gems += FreeGems;
            SaveGems();
            PlayerPrefs.SetInt(FIRST_ENTRY_KEY, 1);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        // Auto-find UI in scene if not assigned
        if (GemText == null)
        {
            GemText = GameObject.Find("GemText")?.GetComponent<TextMeshProUGUI>();
        }

        if (ScoreText == null)
        {
            ScoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        }

        UpdateGemUI();
        UpdateScoreUI();
    }

    // --------- Gem Methods ---------
    public void AddGems(int amount)
    {
        Gems += amount;
        SaveGems();
        UpdateGemUI();
    }

    public bool HasEnoughGems(int amount)
    {
        return Gems >= amount;
    }

    public bool SpendGems(int amount)
    {
        if (!HasEnoughGems(amount)) return false;

        Gems -= amount;
        SaveGems();
        UpdateGemUI();
        return true;
    }

    private void SaveGems()
    {
        PlayerPrefs.SetInt(GEMS_KEY, Gems);
        PlayerPrefs.Save();
    }

    private void LoadGems()
    {
        Gems = PlayerPrefs.GetInt(GEMS_KEY, 0);
    }

    public void UpdateGemUI()
    {
        if (GemText != null)
            GemText.text = "Gems: " + Gems;
    }

    public void SetGemText(TextMeshProUGUI newGemText)
    {
        GemText = newGemText;
        UpdateGemUI();
    }

    // --------- Score Methods ---------
    public void AddScore(int amount)
    {
        TotalScore += amount;
        SaveScore();
        UpdateScoreUI();
    }

    private void SaveScore()
    {
        PlayerPrefs.SetInt(SCORE_KEY, TotalScore);
        PlayerPrefs.Save();
    }

    private void LoadScore()
    {
        TotalScore = PlayerPrefs.GetInt(SCORE_KEY, 0);
    }

    public void UpdateScoreUI()
    {
        if (ScoreText != null)
            ScoreText.text = "Score: " + TotalScore;
    }

    public void SetScoreText(TextMeshProUGUI newScoreText)
    {
        ScoreText = newScoreText;
        UpdateScoreUI();
    }
}