using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartGame()
    {
        int entryGems = 20; // Entry cost

        if (ScoreManager.Instance.SpendGems(entryGems))
        {
            Debug.Log(entryGems + " Gems spent! Starting game...");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Not enough gems to play!");
        }
    }
}
