using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float yPosition;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject doubleLaserPrefabLeft;   // Laser prefab for the left
    [SerializeField] GameObject doubleLaserPrefabRight;  // Laser prefab for the right
    float xMin, xMax;

    bool isLaserUpgraded = false;  // To track if laser is upgraded
    [SerializeField] float laserCooldown = 0.5f; // Cooldown between laser shots
    float nextFireTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        yPosition = transform.position.y;

        // Calculate screen bounds (assuming the camera is orthographic)
        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        xMin = screenBottomLeft.x;  // Left boundary
        xMax = screenTopRight.x;    // Right boundary
    }

    // Update is called once per frame
    void Update()
    {
        // Convert mouse position to world point
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z-axis is 0 to avoid depth issues
        mousePosition.y = yPosition; // Lock the y position

        // Clamp the player's x position to keep it within screen bounds
        mousePosition.x = Mathf.Clamp(mousePosition.x, xMin, xMax);

        // Set player position to the clamped mouse position
        transform.position = mousePosition;

        // Fire laser with cooldown
        if (Input.GetButtonDown("FireLaser") && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + laserCooldown;

            if (isLaserUpgraded)
            {
                FireTripleLaser();
            }
            else
            {
                Instantiate(laser, transform.position, Quaternion.identity);
            }
        }
    }

    void FireTripleLaser()
    {
        // Fire central laser
        Instantiate(laser, transform.position, Quaternion.identity);

        // Fire lasers to the left and right
        Instantiate(doubleLaserPrefabLeft, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 0, 45));
        Instantiate(doubleLaserPrefabRight, transform.position + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 0, -45));
    }

    // Call this method when laser is upgraded by destroying special enemy
    public void UpgradeLaser()
    {
        isLaserUpgraded = true;
    }
}
