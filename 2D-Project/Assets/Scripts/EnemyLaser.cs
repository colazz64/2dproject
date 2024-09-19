using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] float laserSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Move the laser downward towards the player
        transform.position += new Vector3(0, -laserSpeed, 0) * Time.deltaTime;

        // Destroy the laser if it goes off-screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    // Handle collisions with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Destroy the player and the laser
            Destroy(collision.gameObject); // Destroy the player
            Destroy(gameObject); // Destroy the laser
            
            // Call the game over or initiate death sequence
            GameManager.instance.InitiateGameOver();
        }
    }
}
