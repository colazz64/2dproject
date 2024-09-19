using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemy : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float zigzagAmplitude = 3f;
    [SerializeField] float zigzagFrequency = 2f;
    [SerializeField] GameObject enemyLaserPrefab;  // Prefab for the laser
    [SerializeField] Transform firePoint;          // Change this from GameObject to Transform
    [SerializeField] float shootInterval = 2f;

    GameObject player;
    float nextShotTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Zig-zag movement
        float xPosition = Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
        transform.position += new Vector3(xPosition, -speed, 0) * Time.deltaTime;

        // Shoot lasers at player
        if (Time.time > nextShotTime)
        {
            ShootLaser();
            nextShotTime = Time.time + shootInterval;
        }
    }

    void ShootLaser()
    {
        if (player != null)
        {
            // Calculate direction to player
            Vector3 direction = (player.transform.position - transform.position).normalized;
            GameObject laser = Instantiate(enemyLaserPrefab, firePoint.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }
    }
}
