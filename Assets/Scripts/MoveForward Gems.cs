using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardGems : MonoBehaviour
{
    private float speed = 30;
    private PlayerMovement playerMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovementScript != null && !playerMovementScript.isGameOver)
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }
}
