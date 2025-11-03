using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    // Score UI
    public Text scoreText;
    public Text finalScoreText;
    public Text bestScoreText;

    // Coin UI
    public Text coinText;
    public Text finalCoinText;

    // Panels and buttons
    public GameObject gameOverPanel;
    public GameObject retryButton;
    public GameObject startPanel;
    public GameObject startButton;

    // Tap hints
    public GameObject tapLeft;
    public GameObject tapRight;

    // Scores
    public int score;
    private int bestScore;

    // Coins
    public int coins;

    // Audio
    public AudioSource backgroundMusic;
    public AudioSource swooshSound;
    public AudioSource coinSound;

    // Pause System
    private PauseManager pauseManager;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Pause();

        // Automatically get PauseManager on same GameObject
        pauseManager = GetComponent<PauseManager>();
    }

    void Start()
    {
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        retryButton.SetActive(false);
        ShowTapHints(true);

        bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (backgroundMusic != null)
            backgroundMusic.Stop();

        if (pauseManager != null)
            pauseManager.ShowPauseButton(false);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        retryButton.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = score.ToString();

        if (finalCoinText != null)
            finalCoinText.text = "COINS: " + coins.ToString();

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        if (bestScoreText != null)
            bestScoreText.text = "BEST: " + bestScore.ToString();

        if (backgroundMusic != null)
            backgroundMusic.Stop();

        ShowTapHints(true);
        Pause();

        if (pauseManager != null)
            pauseManager.ShowPauseButton(false);
    }

    public void IncreaseScore()
    {
        score++;
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void AddCoinScore(int value)
    {
        coins += value;
        if (coinText != null)
            coinText.text = coins.ToString();

        if (coinSound != null)
            coinSound.Play();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void Play()
    {
        if (swooshSound != null)
            swooshSound.Play();

        score = 0;
        coins = 0;
        scoreText.text = score.ToString();

        if (coinText != null)
            coinText.text = coins.ToString();

        retryButton.SetActive(false);
        gameOverPanel.SetActive(false);
        startPanel.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;
        player.ResetPosition();

        ShowTapHints(false);

        // Clean up old pipes
        Pipes[] pipes = FindObjectsByType<Pipes>(FindObjectsSortMode.None);
        foreach (var pipe in pipes)
            Destroy(pipe.gameObject);

        // Clean up old smoke puffs
        PuffMovement[] puffs = FindObjectsByType<PuffMovement>(FindObjectsSortMode.None);
        foreach (var puff in puffs)
            Destroy(puff.gameObject);

        // Clean up old stars (if they exist)
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Coin");
        foreach (var star in stars)
            Destroy(star);

        // Start background music
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        // Show pause button once game starts
        if (pauseManager != null)
            pauseManager.ShowPauseButton(true);
    }

    public void Retry()
    {
        if (swooshSound != null)
            swooshSound.Play();
        Play();
    }

    private void ShowTapHints(bool show)
    {
        if (tapLeft != null) tapLeft.SetActive(show);
        if (tapRight != null) tapRight.SetActive(show);
    }
}
