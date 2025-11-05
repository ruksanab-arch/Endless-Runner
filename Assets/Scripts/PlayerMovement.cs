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

        // Check if player hits an obstacle or rock
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Rock"))
        {
            isGameOver = true;
            Debug.Log("Game Over!");

            // Play death animation if available
            if (animator != null)
            {
                animator.SetTrigger("Death");
            }

            // Stop player movement
            GetComponent<PlayerMovement>().enabled = false;

            // Restart or show Game Over UI after delay
            Invoke("RestartGame", 2f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}