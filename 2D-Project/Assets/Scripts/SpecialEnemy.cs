using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnemy : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float zigzagAmplitude = 3f;
    [SerializeField] float zigzagFrequency = 2f;
    [SerializeField] GameObject enemyLaserPrefab;  // Prefab for the laser
    [SerializeField] Transform firePoint;          // FirePoint where the laser spawns
    [SerializeField] float shootInterval = 2f;
    [SerializeField] int scoreValue = 50;         // Points awarded for destroying the special enemy

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerLaser")
        {
            // Upgrade the player's laser
            Player player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

            if (player != null)
            {
                player.UpgradeLaser();
            }

            // Increase the player's score for destroying this special enemy
            GameManager.instance.IncreaseScore(scoreValue);

            // Destroy the special enemy and the player's laser
            Destroy(collision.gameObject);  // Destroy the player's laser
            Destroy(gameObject);  // Destroy the special enemy
        }
        else if (collision.gameObject.tag == "Player")
        {
            // Handle player death when the player collides with the special enemy
            GameManager.instance.InitiateGameOver();

            // Destroy the player and the special enemy
            Destroy(collision.gameObject);  // Destroy the player
            Destroy(gameObject);  // Destroy the special enemy
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            // Ignore collisions with normal enemies (do nothing)
            Debug.Log("Special enemy collided with a normal enemy, ignoring collision.");
        }
    }
}
