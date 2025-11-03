using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public Button pauseButton;
    public Text pauseButtonText;
    private bool isPaused = false;

    void Start()
    {
        pauseButton.onClick.AddListener(TogglePause);
        pauseButton.gameObject.SetActive(false);
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            isPaused = false;
            if (pauseButtonText != null)
                pauseButtonText.text = "⏸ Pause";
        }
        else
        {
            Time.timeScale = 0f;
            isPaused = true;
            if (pauseButtonText != null)
                pauseButtonText.text = "▶ Resume";
        }
    }

    // Show or Hide pause button externally
    public void ShowPauseButton(bool show)
    {
        pauseButton.gameObject.SetActive(show);
    }
}
