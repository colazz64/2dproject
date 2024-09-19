using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 2f;
    [SerializeField] GameObject enemyPrefab;        // Regular enemy prefab
    [SerializeField] GameObject specialEnemyPrefab; // Special enemy prefab
    [SerializeField] float specialEnemySpawnChance = 0.2f; // 20% chance to spawn special enemy

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
        float randx = Random.Range(xMin, xMax);

        // Decide whether to spawn regular enemy or special enemy
        if (Random.value <= specialEnemySpawnChance)
        {
            // Spawn special enemy
            Instantiate(specialEnemyPrefab, new Vector3(randx, ySpawn, 0), Quaternion.identity);
        }
        else
        {
            // Spawn regular enemy
            Instantiate(enemyPrefab, new Vector3(randx, ySpawn, 0), Quaternion.identity);
        }
    }
}
