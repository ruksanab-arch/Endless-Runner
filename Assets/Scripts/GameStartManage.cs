using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    // Entry cost in gems
    public int entryCost = 20;

    public void StartGame()
    {
        // Check if player has enough gems to start
        if (ScoreManager.Instance.SpendGems(entryCost))
        {
            Debug.Log(entryCost + " gems spent! Starting game...");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Not enough gems to start the game!");
        }
    }
}