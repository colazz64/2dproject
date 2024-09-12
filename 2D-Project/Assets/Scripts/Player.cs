using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float yPosition;
    [SerializeField] GameObject laser;

    float xMin, xMax;

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

        if (Input.GetButtonDown("FireLaser"))
        {
            Instantiate(laser, transform.position, Quaternion.identity);
        }
    }
}
