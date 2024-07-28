using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // For UI elements

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the UI Text element for displaying score
    public TextMeshProUGUI wicketsText; // Reference to the UI Text element for displaying wickets
    public TextMeshProUGUI timerText; // Reference to the UI Text element for displaying the timer
    public float gameDuration = 180f; // Total game time in seconds (3 minutes)
    public int totalWickets = 5; // Total number of wickets
    public GameObject gameOverPanel; // Reference to the Game Over Panel
    public TextMeshProUGUI gameOverScoreText; // Reference to the Text component in the game over panel for displaying score
    public TextMeshProUGUI gameOverWicketsText; // Reference to the Text component in the game over panel for displaying wickets
    public Button restartButton; // Reference to the Restart Button
    public Button quitButton; // Reference to the Quit Button
    private int currentScore = 0;
    private int currentWickets = 0;
    private float timeRemaining;
    private bool gameOver = false;
    public PlayerController playerController;
    public BatController batController;
    public BallSpawner ballSpawner; // Reference to the BallSpawner

    public TextMeshPro StartText1;
    public TextMeshPro StartText2;
    public TextMeshPro StartText3;
    public TextMeshPro StartText4;

    void Start()
    {
        timeRemaining = gameDuration;
        UpdateUI();

        // Pause the game at the start
        Time.timeScale = 0;

        // Hide the game over panel at the start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Set up button click listeners
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Start waiting for the user to press a key
        StartCoroutine(WaitForStart());
    }

    void Update()
    {
        if (gameOver)
            return;

        // Update UI
        UpdateUI();
    }

    IEnumerator GameTimer()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
            UpdateUI();

            if (timeRemaining <= 0)
            {
                EndGame();
            }
        }
    }

    IEnumerator WaitForStart()
    {
        // Wait until the player presses any key
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        // Hide the startup text
        StartText1.gameObject.SetActive(false);
        StartText2.gameObject.SetActive(false);
        StartText3.gameObject.SetActive(false);
        StartText4.gameObject.SetActive(false);

        // Resume the game
        Time.timeScale = 1;

        // Start the game timer
        StartCoroutine(GameTimer());
    }

    public void AddScore(int amount)
    {
        if (gameOver)
            return;

        currentScore += amount;
        UpdateUI();
    }

    public void AddWicket()
    {
        if (gameOver)
            return;

        currentWickets += 1;
        UpdateUI();
        playerController.ResetValues();
        batController.ResetValues();

        if (currentWickets >= totalWickets)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameOver = true;
        // Show the game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            // Update the game over panel with final score and wickets
            if (gameOverScoreText != null)
                gameOverScoreText.text = "Final Score: " + currentScore;

            if (gameOverWicketsText != null)
                gameOverWicketsText.text = "Final Wickets: " + currentWickets;
        }

        // Stop the game
        Time.timeScale = 0;
        Debug.Log("Game Over!");
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;

        if (wicketsText != null)
            wicketsText.text = "Wickets: " + currentWickets;

        if (timerText != null)
            timerText.text = "Time: " + Mathf.Max(timeRemaining, 0).ToString("0");
    }

    void RestartGame()
    {
        Time.timeScale = 1; // Resume game time
        ResetGameState(); // Reset game state
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    void ResetGameState()
    {
        // Reset GameManager variables
        currentScore = 0;
        currentWickets = 0;
        timeRemaining = gameDuration;
        gameOver = false;

        // Reset BallSpawner
        if (ballSpawner != null)
        {
            ballSpawner.ResetBallCounter();
        }

        // Reset PlayerController and BatController
        if (playerController != null)
        {
            playerController.ResetValues();
        }

        if (batController != null)
        {
            batController.ResetValues();
        }

        // Hide the game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }




        UpdateUI();
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
#else
        Application.Quit(); // Quit the application
#endif
    }




}
