using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private GameSceneAudio gameAudio;
    
    // ADD THIS - Reference to GameOverGlitch script
    public GameOverGlitch gameOverGlitch;

    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern - as taught in slides
        if (instance == null)
            instance = this;

        gameAudio = FindObjectOfType<GameSceneAudio>();
        if (gameAudio == null)
        {
            GameObject audioObj = new GameObject("GameSceneAudio");
            gameAudio = audioObj.AddComponent<GameSceneAudio>();
        }
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

        if (gameAudio != null)
        {
            gameAudio.TriggerGameOver();
        }
            
        if (gameOverGlitch != null)
        {
            gameOverGlitch.TriggerGameOver(score);
        }
        else
        {
            // Fallback if no glitch script assigned
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }

        // Stop time
        Time.timeScale = 0f;

        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("GAME OVER!");
    }
}