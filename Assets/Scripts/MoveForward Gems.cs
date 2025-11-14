using System.Collections;
using UnityEngine;

public class MoveForwardGems : MonoBehaviour
{
    private float speed = 30;
    private PlayerMovement playerMovementScript;

    void Start()
    {
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        // Keep searching until Player is found
        while (playerMovementScript == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                playerMovementScript = playerObj.GetComponent<PlayerMovement>();
                Debug.Log("Player found successfully!");
                yield break;
            }

            Debug.Log("Player NOT found... retrying");
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Update()
    {
        if (playerMovementScript != null && !playerMovementScript.isGameOver)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }
}
