using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private Animator animator;
    private bool isGameOver = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isGameOver) return; // Prevent multiple triggers

        // 🧱 If player hits obstacle or rock → Game Over
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Rock"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            // Play death animation if available
            if (animator != null)
                animator.SetTrigger("Death");

            // Stop player movement
            GetComponent<PlayerMovement>().enabled = false;

            // Restart or show Game Over UI after delay
            Invoke("RestartGame", 2f);
        }

        // 💎 If player collects a gem → Add score and destroy gem
        if (collision.gameObject.CompareTag("Gem"))
        {
            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(10); // Add 10 points

            Destroy(collision.gameObject); // Remove gem
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}