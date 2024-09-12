
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 2f;
    [SerializeField] GameObject enemyPrefab;

    float xMin;
    float xMax;
    float ySpawn;
    // Start is called before the first frame update
    void Start()
    {
        xMin = Camera.main.ViewportToWorldPoint(new Vector3(.1f, 0, 0)).x;
        xMax = Camera.main.ViewportToWorldPoint(new Vector3(.9f, 0, 0)).x;
        ySpawn = Camera.main.ViewportToWorldPoint(new Vector3(0, 1.25f, 0)).y;

        InvokeRepeating("SpawnEnemy", 0, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        float randx = Random.Range(xMin,xMax);
        Instantiate(enemyPrefab, new Vector3(randx, ySpawn, 0), Quaternion.identity);
    }


}


