using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene
using TMPro; // For TextMesh Pro

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coinsCollected = 0; // Number of coins collected
    public int totalCoins = 25; // Total coins to win

    public float countdownTimer = 180f; // Timer in seconds
    public TMP_Text coinText; // TextMesh Pro for coin display
    public TMP_Text timerText; // TextMesh Pro for timer display

    public GameObject levelCompletePanel; // Reference to the Level Complete Panel UI

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateCoinUI();
        UpdateTimerUI();
    }

    void Update()
    {
        // Update timer
        countdownTimer -= Time.deltaTime;
        UpdateTimerUI();

        // End game if timer runs out
        if (countdownTimer <= 0)
        {
            EndGame(false);
        }
    }

    public void CollectCoin()
    {
        // Increment coin count and update UI
        coinsCollected++;
        UpdateCoinUI();

        // Check if all coins are collected
        if (coinsCollected >= totalCoins)
        {
            ShowLevelCompleteScreen();
        }
    }

    void UpdateCoinUI()
    {
        coinText.text = $"Coins: {coinsCollected}/{totalCoins}";
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(countdownTimer / 60);
        int seconds = Mathf.FloorToInt(countdownTimer % 60);
        timerText.text = $"Timer: {minutes:00}:{seconds:00}";
    }

    void ShowLevelCompleteScreen()
    {
        // Pause the game and show the Level Complete screen
        Time.timeScale = 0; // Stop the game
        levelCompletePanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Reload the scene to restart the game
        Time.timeScale = 1; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void EndGame(bool won)
    {
        if (!won)
        {
            Debug.Log("Time ran out! Game Over.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}