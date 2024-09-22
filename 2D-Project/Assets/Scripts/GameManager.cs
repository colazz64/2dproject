using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score = 0;
    bool isGameOver = false;

    public static GameManager instance;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] private GameObject upgradeIcon;

    private void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.SetActive(false);
        UpdateScoreText();  // Update score text at the start of the game
        upgradeIcon.SetActive(false);  // Hide the upgrade icon initially
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    public void InitiateGameOver()
    {
        isGameOver = true;
        gameOverText.SetActive(true);
    }

    // Show the upgrade icon when the laser is upgraded
    public void ShowUpgradeIcon()
    {
        if (upgradeIcon != null)
        {
            upgradeIcon.SetActive(true);  // Show the upgrade icon
        }
    }

    // Hide the upgrade icon when the timer ends
    public void HideUpgradeIcon()
    {
        if (upgradeIcon != null)
        {
            upgradeIcon.SetActive(false);  // Hide the upgrade icon
        }
    }
}
