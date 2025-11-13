using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject gemPrefab;
    private Vector3 spawnPosO = new Vector3(0, 0, 25);
    private Vector3 spawnPosG = new Vector3(0, 1, 55);
    private float startDelay = 2;
    private float repeatRate = 2;
    private PlayerMovement playerMovementScript;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnGem", startDelay, repeatRate);
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SpawnObstacle()
    {
        if (playerMovementScript != null && !playerMovementScript.isGameOver)
        {
            Instantiate(obstaclePrefab, spawnPosO, obstaclePrefab.transform.rotation);
        }
         

    }
    void SpawnGem()
    {
        if (playerMovementScript != null && !playerMovementScript.isGameOver)
        {
            Instantiate(gemPrefab, spawnPosG, gemPrefab.transform.rotation);
        }
        
    }
}