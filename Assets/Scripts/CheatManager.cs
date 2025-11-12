using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    void Update()
    {
        // Press "C" key to add 100 gems
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddGems(100);
                Debug.Log("Cheat activated: +100 Gems");
            }
        }
    }
}