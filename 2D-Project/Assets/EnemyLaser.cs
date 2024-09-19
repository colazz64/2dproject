using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] float laserSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // Move the laser down the screen
        transform.position += new Vector3(0, -laserSpeed, 0) * Time.deltaTime;
        
        // Destroy the laser if it goes off-screen (optional)
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}
