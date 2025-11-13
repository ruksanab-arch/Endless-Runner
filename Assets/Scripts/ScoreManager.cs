using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private const string GEMS_KEY = "PlayerGems";
    public int Gems { get; private set; }

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI GemText; // Drag your LobbyScene text here (optional)

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the "GemText_Game" object automatically in new scenes
        if (GemText == null)
        {
            var foundText = GameObject.Find("GemText_Game");
            if (foundText != null)
            {
                GemText = foundText.GetComponent<TextMeshProUGUI>();
                UpdateGemUI();
            }
        }
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

    public bool SpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            SaveGems();
            UpdateGemUI();
            Debug.Log(amount + " gems spent!");
            return true;
        }
        else
        {
            Debug.Log("Not enough gems to spend!");
            return false;
        }
    }

    public void CheatAddGems()
    {
        AddGems(50);
        Debug.Log("Cheat activated! +50 gems");
    }
}