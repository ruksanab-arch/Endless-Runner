using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject gemPrefab;
    private Vector3 spawnPosO = new Vector3(0, 0, 25);
    private Vector3 spawnPosG = new Vector3(0, 1, 55);
    private float startDelay = 3;
    private float repeatRate = 3;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        InvokeRepeating("SpawnGem", startDelay, repeatRate);
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SpawnObstacle()
    {
         Instantiate(obstaclePrefab, spawnPosO, obstaclePrefab.transform.rotation);

    }
    void SpawnGem()
    {
        Instantiate(gemPrefab, spawnPosG, gemPrefab.transform.rotation);
    }
}
