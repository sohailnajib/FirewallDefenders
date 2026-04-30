using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int waveNumber = 0;

    // UI elements - we'll connect these in Inspector
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    public Slider firewallHPBar;
    

    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern - as taught in slides
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        if (isGameOver) return;

        // Update UI every frame
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (waveText != null)
            waveText.text = "Wave: " + waveNumber;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void UpdateWave(int wave)
    {
        waveNumber = wave;
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthText != null)
            healthText.text = "Firewall HP: " + current + "/" + max;
        if (firewallHPBar != null)
            firewallHPBar.value = current;
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Show game over panel
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Stop time
        Time.timeScale = 0f;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("GAME OVER!");
    }
}