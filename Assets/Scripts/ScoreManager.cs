using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    // Start is called before the first frame update
    private const string COINS_KEY = "PlayerCoins";
    public int Coins { get; private set; }

    [Header("UI Reference (Optional)")]
    public TextMeshProUGUI coinText; // Drag UI text here in Inspector

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
        LoadCoins();
    }

    private void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        SaveCoins();
        UpdateCoinUI();
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt(COINS_KEY, Coins);
        PlayerPrefs.Save();
    }

    public void LoadCoins()
    {
        Coins = PlayerPrefs.GetInt(COINS_KEY, 0);
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + Coins.ToString();
    }
}
