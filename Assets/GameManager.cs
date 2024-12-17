using UnityEngine;
using UnityEngine.SceneManagement; // For reloading scenes
using TMPro; // For TextMesh Pro

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public int coinsCollected = 0; // Number of coins collected
    public int totalCoins = 25; // Total coins needed to win the game

    public float countdownTimer = 180f; // 3 minutes in seconds

    public TMP_Text coinText; // TextMesh Pro UI element for coin counter
    public TMP_Text timerText; // TextMesh Pro UI element for countdown timer

    void Awake()
    {
        // Singleton pattern to ensure one GameManager instance
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
        // Initialize the UI at the start of the game
        UpdateCoinUI();
        UpdateTimerUI();
    }

    void Update()
    {
        // Update the countdown timer
        countdownTimer -= Time.deltaTime;
        UpdateTimerUI();

        // End the game if the timer runs out
        if (countdownTimer <= 0)
        {
            EndGame(false); // Player loses
        }
    }

    public void CollectCoin()
    {
        // Increment the coin count
        coinsCollected++;
        UpdateCoinUI();

        // Check if all coins are collected
        if (coinsCollected >= totalCoins)
        {
            EndGame(true); // Player wins
        }
    }

    void UpdateCoinUI()
    {
        // Update the coin counter text
        coinText.text = $"Coins: {coinsCollected}/{totalCoins}";
    }

    void UpdateTimerUI()
    {
        // Format the timer to show minutes and seconds
        int minutes = Mathf.FloorToInt(countdownTimer / 60);
        int seconds = Mathf.FloorToInt(countdownTimer % 60);
        timerText.text = $"Timer: {minutes:00}:{seconds:00}";
    }

    void EndGame(bool won)
    {
        // Display a win or lose message in the Console
        if (won)
        {
            Debug.Log("You collected all the coins! You win!");
        }
        else
        {
            Debug.Log("Time ran out! Game over.");
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}