using System.Collections;
using UnityEngine;
using TMPro;  // Import TextMeshPro

public class Player : MonoBehaviour
{
    float yPosition;
    [SerializeField] GameObject laser;                // Default laser prefab
    [SerializeField] GameObject upgradedLaserPrefab;  // Upgraded laser prefab
    [SerializeField] GameObject doubleLaserPrefabLeft;   // Left laser prefab
    [SerializeField] GameObject doubleLaserPrefabRight;  // Right laser prefab

    [SerializeField] float laserUpgradeDuration = 5f;    // Set the upgrade duration to 5 seconds
    [SerializeField] TextMeshProUGUI upgradeTimerText;   // Reference to the upgrade timer text
    [SerializeField] Sprite upgradedPlayerSprite;        // Sprite for the upgraded player
    [SerializeField] Sprite defaultPlayerSprite;         // Default sprite for the player

    private SpriteRenderer spriteRenderer;  // Reference to SpriteRenderer
    float xMin, xMax;
    bool isLaserUpgraded = false;
    [SerializeField] float laserCooldown = 0.5f;         // Cooldown between laser shots
    float nextFireTime = 0f;

    private float remainingTime = 0f;  // Remaining time for the laser upgrade

    Coroutine upgradeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        yPosition = transform.position.y;

        // Get the sprite renderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Calculate screen bounds (assuming the camera is orthographic)
        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        xMin = screenBottomLeft.x;  // Left boundary
        xMax = screenTopRight.x;    // Right boundary

        // Hide the timer text initially
        upgradeTimerText.gameObject.SetActive(false);

        // Set default player sprite
        spriteRenderer.sprite = defaultPlayerSprite;
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
                FireSingleLaser();
            }
        }
    }

    void FireSingleLaser()
    {
        // Fire the default laser
        Instantiate(laser, transform.position, Quaternion.identity);
    }

    void FireTripleLaser()
    {
        // Use the upgraded laser for all lasers if the player is upgraded
        GameObject laserPrefabToUse = isLaserUpgraded ? upgradedLaserPrefab : laser;

        // Fire central laser
        Instantiate(laserPrefabToUse, transform.position, Quaternion.identity);

        // Fire lasers to the left and right
        Instantiate(laserPrefabToUse, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 0, 45));
        Instantiate(laserPrefabToUse, transform.position + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 0, -45));
    }

    // Method to upgrade the laser and change player appearance
    public void UpgradeLaser()
    {
        isLaserUpgraded = true;

        // Add time to the upgrade timer instead of resetting it
        remainingTime += laserUpgradeDuration;

        // Change the player sprite to the upgraded one
        spriteRenderer.sprite = upgradedPlayerSprite;

        // Start or continue the timer display
        upgradeTimerText.gameObject.SetActive(true);

        // If an upgrade coroutine is already running, don't start a new one, but let the existing one continue
        if (upgradeCoroutine == null)
        {
            upgradeCoroutine = StartCoroutine(RevertLaserAfterDelay());
        }
    }

    // Coroutine to revert laser back to normal after the accumulated time expires
    IEnumerator RevertLaserAfterDelay()
    {
        while (remainingTime > 0)
        {
            // Update the timer text
            upgradeTimerText.text = "Laser Upgrade: " + remainingTime.ToString("F1");  // Display time with 1 decimal
            remainingTime -= Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Hide the timer text and revert the laser once time is up
        upgradeTimerText.gameObject.SetActive(false);
        isLaserUpgraded = false;
        spriteRenderer.sprite = defaultPlayerSprite;

        // Reset the coroutine reference to null when done
        upgradeCoroutine = null;
    }
}
