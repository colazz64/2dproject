using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    float yPosition;
    [SerializeField] GameObject laser;
    [SerializeField] GameObject upgradedLaserPrefab;
    [SerializeField] float laserUpgradeDuration = 5f;
    [SerializeField] TextMeshProUGUI upgradeTimerText;
    [SerializeField] Sprite upgradedPlayerSprite;
    [SerializeField] Sprite defaultPlayerSprite;

    private SpriteRenderer spriteRenderer;
    float xMin, xMax;
    bool isLaserUpgraded = false;
    [SerializeField] float laserCooldown = 0.5f;
    float nextFireTime = 0f;

    private float remainingTime = 0f;
    Coroutine upgradeCoroutine;

    void Start()
    {
        yPosition = transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        xMin = screenBottomLeft.x;
        xMax = screenTopRight.x;

        upgradeTimerText.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        mousePosition.y = yPosition;
        mousePosition.x = Mathf.Clamp(mousePosition.x, xMin, xMax);
        transform.position = mousePosition;

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
        Instantiate(laser, transform.position, Quaternion.identity);
    }

    void FireTripleLaser()
    {
        Instantiate(upgradedLaserPrefab, transform.position, Quaternion.identity);
        Instantiate(upgradedLaserPrefab, transform.position + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 0, 45));
        Instantiate(upgradedLaserPrefab, transform.position + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 0, -45));
    }

    public void UpgradeLaser()
    {
        isLaserUpgraded = true;
        remainingTime = laserUpgradeDuration;

        spriteRenderer.sprite = upgradedPlayerSprite;
        upgradeTimerText.gameObject.SetActive(true);

        // Show upgrade icon via GameManager
        GameManager.instance.ShowUpgradeIcon();

        if (upgradeCoroutine != null)
        {
            StopCoroutine(upgradeCoroutine);
        }
        upgradeCoroutine = StartCoroutine(RevertLaserAfterDelay());
    }

    IEnumerator RevertLaserAfterDelay()
    {
        while (remainingTime > 0)
        {
            upgradeTimerText.text = "Laser Upgrade: " + remainingTime.ToString("F1");
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        upgradeTimerText.gameObject.SetActive(false);

        // Revert back to default state
        isLaserUpgraded = false;
        spriteRenderer.sprite = defaultPlayerSprite;

        // Hide the upgrade icon when the timer ends
        GameManager.instance.HideUpgradeIcon();

        upgradeCoroutine = null;
    }
}
