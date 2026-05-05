using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameOverGlitch : MonoBehaviour
{
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverTitleText;
    public TMP_Text finalScoreText;
    
    [Header("Buttons")]
    public Button restartButton;
    public TMP_Text restartButtonText;
    public Image restartButtonImage;
    public Button exitButton;
    public TMP_Text exitButtonText;
    public Image exitButtonImage;
    
    public Image panelBackground;
    
    [Header("Glitch Settings")]
    public Color glitchColor1 = Color.red;
    public Color glitchColor2 = new Color(1f, 0f, 0.5f);
    public Color finalColor = new Color(1f, 0.2f, 0.2f);
    public float glitchDuration = 1.5f;
    public float buttonDelay = 0.8f;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.03f;
    
    [Header("Button Colors (Font size NOT controlled by script)")]
    public Color buttonFinalColor = new Color(0.1f, 0.15f, 0.25f, 0.9f);
    public Color buttonTextFinalColor = new Color(0f, 1f, 0.8f);
    
    private Vector2 originalTitlePos;
    private string originalTitleText = "GAME OVER";
    private bool isGlitching = false;
    
    private Vector3 originalRestartScale;
    private Vector3 originalExitScale;
    private Vector2 originalRestartPos;
    private Vector2 originalExitPos;
    
    private string originalRestartText;
    private string originalExitText;
    private float originalRestartFontSize;
    private float originalExitFontSize;
    
    void Start()
    {
        Debug.Log("GameOverGlitch Start() called");
        
        if (gameOverTitleText != null)
        {
            originalTitlePos = gameOverTitleText.rectTransform.anchoredPosition;
            originalTitleText = gameOverTitleText.text;
        }
        
        if (restartButton != null)
        {
            originalRestartScale = restartButton.transform.localScale;
            originalRestartPos = restartButton.GetComponent<RectTransform>().anchoredPosition;
            
            if (restartButtonText != null)
            {
                originalRestartText = restartButtonText.text;
                originalRestartFontSize = restartButtonText.fontSize;
            }
        }
        
        if (exitButton != null)
        {
            originalExitScale = exitButton.transform.localScale;
            originalExitPos = exitButton.GetComponent<RectTransform>().anchoredPosition;
            
            if (exitButtonText != null)
            {
                originalExitText = exitButtonText.text;
                originalExitFontSize = exitButtonText.fontSize;
            }
        }
        
        HideButtons();
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);
    }
    
    void HideButtons()
    {
        if (restartButton != null)
        {
            restartButton.interactable = false;
            
            if (restartButtonImage != null)
            {
                Color tempColor = restartButtonImage.color;
                tempColor.a = 0f;
                restartButtonImage.color = tempColor;
            }
            
            if (restartButtonText != null)
                restartButtonText.alpha = 0f;
            
            restartButton.transform.localScale = Vector3.zero;
        }
        
        if (exitButton != null)
        {
            exitButton.interactable = false;
            
            if (exitButtonImage != null)
            {
                Color tempColor = exitButtonImage.color;
                tempColor.a = 0f;
                exitButtonImage.color = tempColor;
            }
            
            if (exitButtonText != null)
                exitButtonText.alpha = 0f;
            
            exitButton.transform.localScale = Vector3.zero;
        }
    }
    
    public void TriggerGameOver(int finalScore)
    {
        Debug.Log("TriggerGameOver called with score: " + finalScore);
        
        if (isGlitching) return;
        isGlitching = true;
        
        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + finalScore;
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            StartCoroutine(GlitchSequence());
        }
    }
    
    IEnumerator GlitchSequence()
    {
        float elapsed = 0;
        
        if (panelBackground != null)
        {
            panelBackground.color = Color.white;
            yield return new WaitForSecondsRealtime(0.1f);
            panelBackground.color = new Color(0.05f, 0.05f, 0.1f, 0.95f);
        }
        
        while (elapsed < glitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            
            if (gameOverTitleText != null && intensity > 0.4f)
            {
                gameOverTitleText.rectTransform.anchoredPosition = new Vector2(
                    originalTitlePos.x + Random.Range(-15f, 15f),
                    originalTitlePos.y + Random.Range(-8f, 8f)
                );
                
                gameOverTitleText.color = intensity > 0.7f ? glitchColor1 : glitchColor2;
                
                if (intensity > 0.8f)
                {
                    char[] chars = originalTitleText.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (Random.Range(0, 3) == 0)
                            chars[i] = "!@#$%^&*"[Random.Range(0, 8)];
                    }
                    gameOverTitleText.text = new string(chars);
                }
                else
                {
                    gameOverTitleText.text = originalTitleText;
                }
            }
            else if (gameOverTitleText != null)
            {
                gameOverTitleText.rectTransform.anchoredPosition = originalTitlePos;
                gameOverTitleText.text = originalTitleText;
            }
            
            if (finalScoreText != null && intensity > 0.5f)
            {
                finalScoreText.color = new Color(
                    Random.Range(0.8f, 1f),
                    Random.Range(0f, 0.5f),
                    Random.Range(0f, 0.5f)
                );
            }
            else if (finalScoreText != null)
            {
                finalScoreText.color = Color.white;
            }
            
            elapsed += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
        if (gameOverTitleText != null)
        {
            gameOverTitleText.rectTransform.anchoredPosition = originalTitlePos;
            gameOverTitleText.text = originalTitleText;
            gameOverTitleText.color = finalColor;
            gameOverTitleText.fontSize = 72;
        }
        
        if (finalScoreText != null)
        {
            finalScoreText.color = new Color(1f, 0.5f, 0.5f);
        }
        
        yield return new WaitForSecondsRealtime(buttonDelay);
        
        StartCoroutine(ButtonsGlitchAppearance());
    }
    
    IEnumerator ButtonsGlitchAppearance()
    {
        float buttonGlitchDuration = 0.6f;
        float elapsed = 0;
        
        if (restartButton != null) restartButton.gameObject.SetActive(true);
        if (exitButton != null) exitButton.gameObject.SetActive(true);
        
        while (elapsed < buttonGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            float t = elapsed / buttonGlitchDuration;
            
            AnimateButtonGlitch(restartButton, restartButtonImage, restartButtonText, intensity, t, originalRestartScale, originalRestartPos);
            AnimateButtonGlitch(exitButton, exitButtonImage, exitButtonText, intensity, t, originalExitScale, originalExitPos);
            
            elapsed += 0.05f;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
        FinalizeButton(restartButton, restartButtonImage, restartButtonText, originalRestartText, originalRestartScale, originalRestartPos, originalRestartFontSize);
        FinalizeButton(exitButton, exitButtonImage, exitButtonText, originalExitText, originalExitScale, originalExitPos, originalExitFontSize);
        
        if (restartButton != null) restartButton.interactable = true;
        if (exitButton != null) exitButton.interactable = true;
        
        if (restartButton != null) StartCoroutine(PulseButton(restartButton, restartButtonText));
        if (exitButton != null) StartCoroutine(PulseButton(exitButton, exitButtonText));
    }
    
    void AnimateButtonGlitch(Button button, Image buttonImage, TMP_Text buttonText, float intensity, float t, Vector3 originalScale, Vector2 originalPos)
    {
        if (button == null) return;
        
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        
        if (intensity > 0.5f && t < 0.9f)
        {
            if (buttonRect != null)
            {
                buttonRect.anchoredPosition = new Vector2(
                    originalPos.x + Random.Range(-8f, 8f),
                    originalPos.y + Random.Range(-4f, 4f)
                );
            }
            
            if (buttonImage != null)
            {
                buttonImage.color = new Color(
                    Random.Range(0.8f, 1f),
                    Random.Range(0f, 0.3f),
                    Random.Range(0f, 0.3f),
                    t
                );
            }
            
            if (buttonText != null)
            {
                if (intensity > 0.7f)
                {
                    string glitchedText = buttonText.text == "RESTART" ? "REST RT" : "EX T";
                    char[] chars = glitchedText.ToCharArray();
                    for (int i = 0; i < chars.Length; i++)
                    {
                        if (Random.Range(0, 3) == 0)
                            chars[i] = "!@#$"[Random.Range(0, 4)];
                    }
                    buttonText.text = new string(chars);
                }
                buttonText.alpha = t;
                buttonText.color = Color.red;
            }
            
            float targetScale = Mathf.Lerp(0f, 1f, t);
            float glitchScale = Mathf.Min(targetScale + Random.Range(-0.1f, 0.1f), 1f);
            button.transform.localScale = new Vector3(glitchScale, glitchScale, 1f);
        }
        else
        {
            float targetScale = Mathf.Lerp(0f, 1f, t);
            button.transform.localScale = new Vector3(targetScale, targetScale, 1f);
            
            if (buttonRect != null)
            {
                buttonRect.anchoredPosition = Vector2.Lerp(
                    new Vector2(originalPos.x, originalPos.y),
                    originalPos,
                    t
                );
            }
            
            if (buttonText != null)
            {
                buttonText.alpha = t;
            }
            
            if (buttonImage != null)
            {
                Color c = buttonFinalColor;
                c.a = t;
                buttonImage.color = c;
            }
        }
    }
    
    void FinalizeButton(Button button, Image buttonImage, TMP_Text buttonText, string originalText, Vector3 originalScale, Vector2 originalPos, float originalFontSize)
    {
        if (button == null) return;
        
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        
        if (buttonRect != null)
        {
            buttonRect.anchoredPosition = originalPos;
        }
        
        button.transform.localScale = originalScale;
        
        if (buttonImage != null)
        {
            buttonImage.color = buttonFinalColor;
        }
        
        if (buttonText != null)
        {
            buttonText.text = originalText;  
            buttonText.alpha = 1f;
            buttonText.color = buttonTextFinalColor;
            buttonText.fontSize = originalFontSize;  
        }
    }
    
    IEnumerator PulseButton(Button button, TMP_Text buttonText)
    {
        if (button == null) yield break;
        
        Vector3 originalScale = button.transform.localScale;
        
        while (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            float pulse = 1 + (Mathf.Sin(Time.realtimeSinceStartup * pulseSpeed) * pulseAmount);
            button.transform.localScale = new Vector3(
                originalScale.x * pulse,
                originalScale.y * pulse,
                1f
            );
            
            if (buttonText != null)
            {
                float alpha = 0.7f + Mathf.Sin(Time.realtimeSinceStartup * pulseSpeed * 1.5f) * 0.3f;
                buttonText.color = new Color(0f, 1f, 0.8f, alpha);
            }
            
            yield return null;
        }
        
        button.transform.localScale = originalScale;
    }
    
    void RestartGame()
    {
        Debug.Log("RestartGame called");
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    void ExitGame()
    {
        Debug.Log("ExitGame called");
        Time.timeScale = 1f;
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}