using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using TMPro; 

public class GlitchEffects : MonoBehaviour
{
    [Header("Background")]
    public Image backgroundImage;
    
    [Header("Text Elements (TextMeshPro)")]
    public TMP_Text[] glitchTexts;  
    
    [Header("Start Button")]
    public Button startButton;
    public Image buttonImage;
    public TMP_Text buttonText;  
    
    [Header("Timing")]
    public float bgGlitchDuration = 2f;
    public float textGlitchDuration = 1.5f;
    public float buttonGlitchDuration = 1f;
    
    private Vector3 originalBgScale;
    private Vector2 originalBgPos;
    private Color originalBgColor;
    private string[] originalTexts;
    private Color[] originalTextColors;
    private Vector2[] originalTextPositions;
    
    // Store original button position
    private Vector2 originalButtonPos;
    private Vector2 originalButtonTextPos;
    
    void Start()
    {
        if (backgroundImage != null)
        {
            originalBgScale = backgroundImage.rectTransform.localScale;
            originalBgPos = backgroundImage.rectTransform.anchoredPosition;
            originalBgColor = backgroundImage.color;
        }
        
        if (glitchTexts != null && glitchTexts.Length > 0)
        {
            originalTexts = new string[glitchTexts.Length];
            originalTextColors = new Color[glitchTexts.Length];
            originalTextPositions = new Vector2[glitchTexts.Length];
            
            for (int i = 0; i < glitchTexts.Length; i++)
            {
                if (glitchTexts[i] != null)
                {
                    originalTexts[i] = glitchTexts[i].text;
                    originalTextColors[i] = glitchTexts[i].color;
                    originalTextPositions[i] = glitchTexts[i].rectTransform.anchoredPosition;
                    
                    glitchTexts[i].alpha = 0f;
                }
            }
        }
        
        if (startButton != null)
        {
            originalButtonPos = startButton.GetComponent<RectTransform>().anchoredPosition;
            
            CanvasGroup buttonCanvasGroup = startButton.GetComponent<CanvasGroup>();
            if (buttonCanvasGroup == null)
            {
                buttonCanvasGroup = startButton.gameObject.AddComponent<CanvasGroup>();
            }
            buttonCanvasGroup.alpha = 0f;
            buttonCanvasGroup.interactable = false;
            buttonCanvasGroup.blocksRaycasts = false;
        }
        
        if (buttonText != null)
        {
            originalButtonTextPos = buttonText.rectTransform.anchoredPosition;
            buttonText.alpha = 0f;
        }
        
        if (buttonImage != null)
        {
            Color tempColor = buttonImage.color;
            tempColor.a = 0f;
            buttonImage.color = tempColor;
        }
        
        if (startButton != null)
        {
            startButton.interactable = false;
        }
        
        StartCoroutine(FullGlitchSequence());
    }
    
    IEnumerator FullGlitchSequence()
    {
        Debug.Log("Step 1: Glitching background...");
        yield return StartCoroutine(GlitchBackground());
        yield return new WaitForSeconds(0.3f);
        
        Debug.Log("Step 2: Glitching text...");
        yield return StartCoroutine(GlitchText());
        yield return new WaitForSeconds(0.3f);
        
        Debug.Log("Step 3: Glitching button...");
        yield return StartCoroutine(GlitchButton());
        
        if (startButton != null)
        {
            startButton.interactable = true;
            Debug.Log("Start button ready!");
        }
    }
    
    IEnumerator GlitchBackground()
    {
        float elapsed = 0;
        
        while (elapsed < bgGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            
            if (intensity > 0.5f && backgroundImage != null)
            {
                backgroundImage.rectTransform.anchoredPosition = new Vector2(
                    Random.Range(-20f, 20f),
                    Random.Range(-10f, 10f)
                );
                
                backgroundImage.color = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0f, 0.3f),
                    Random.Range(0f, 0.5f),
                    1
                );
            }
            else if (backgroundImage != null)
            {
                backgroundImage.rectTransform.anchoredPosition = originalBgPos;
                backgroundImage.color = originalBgColor;
            }
            
            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        
        if (backgroundImage != null)
        {
            backgroundImage.rectTransform.anchoredPosition = originalBgPos;
            backgroundImage.color = originalBgColor;
        }
    }
    
    IEnumerator GlitchText()
    {
        float elapsed = 0;
        bool hasShownText = false;
        
        while (elapsed < textGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            
            // Loop through each text element individually
            for (int i = 0; i < glitchTexts.Length; i++)
            {
                TMP_Text textElement = glitchTexts[i];
                if (textElement == null) continue;
                
                if (intensity > 0.6f)
                {
                    // Strong glitch - random characters
                    if (originalTexts != null && i < originalTexts.Length)
                    {
                        textElement.text = GetRandomGlitchText(originalTexts[i]);
                    }
                    textElement.color = new Color(
                        Random.Range(0.8f, 1f),
                        Random.Range(0f, 0.5f),
                        Random.Range(0f, 0.5f),
                        1
                    );
                    
                    // Fade in gradually during glitch
                    float targetAlpha = Mathf.Lerp(0f, 1f, elapsed / textGlitchDuration);
                    textElement.alpha = targetAlpha;
                    hasShownText = true;
                }
                else if (intensity > 0.3f)
                {
                    textElement.color = new Color(0.7f, 0.5f, 1f, 1f);
                    textElement.alpha = Mathf.Lerp(0f, 1f, elapsed / textGlitchDuration);
                }
                else
                {
                    if (originalTexts != null && i < originalTexts.Length)
                    {
                        textElement.text = originalTexts[i];
                    }
                    textElement.color = originalTextColors[i];
                    textElement.alpha = Mathf.Lerp(0f, 1f, elapsed / textGlitchDuration);
                }
            }
            
            elapsed += 0.08f;
            yield return new WaitForSeconds(0.08f);
        }
        
        for (int i = 0; i < glitchTexts.Length; i++)
        {
            if (glitchTexts[i] != null && originalTexts != null && i < originalTexts.Length)
            {
                glitchTexts[i].text = originalTexts[i];
                glitchTexts[i].color = originalTextColors[i];
                glitchTexts[i].alpha = 1f; // Fully visible
            }
        }
    }
    
    IEnumerator GlitchButton()
    {
        float elapsed = 0;
        string originalButtonText = "";
        
        if (buttonText != null)
        {
            originalButtonText = buttonText.text;
            buttonText.alpha = 0f;
        }
        
        // Get canvas group for button
        CanvasGroup buttonCanvasGroup = startButton != null ? startButton.GetComponent<CanvasGroup>() : null;
        
        while (elapsed < buttonGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            float targetAlpha = Mathf.Lerp(0f, 1f, elapsed / buttonGlitchDuration);
            
            if (buttonImage != null)
            {
                if (intensity > 0.5f)
                {
                    Color imgColor = new Color(
                        Random.Range(0.8f, 1f),
                        Random.Range(0f, 0.3f),
                        Random.Range(0f, 0.3f),
                        targetAlpha
                    );
                    buttonImage.color = imgColor;
                }
                else
                {
                    Color imgColor = Color.white;
                    imgColor.a = targetAlpha;
                    buttonImage.color = imgColor;
                }
            }
            
            if (buttonText != null)
            {
                if (intensity > 0.7f)
                {
                    buttonText.text = GetRandomGlitchText(originalButtonText);
                    buttonText.color = Color.red;
                    buttonText.alpha = targetAlpha;
                }
                else if (intensity > 0.3f)
                {
                    buttonText.color = new Color(1f, 0.5f, 0.5f, 1);
                    buttonText.alpha = targetAlpha;
                }
                else
                {
                    buttonText.text = originalButtonText;
                    buttonText.color = Color.white;
                    buttonText.alpha = targetAlpha;
                }
            }
            
            if (buttonCanvasGroup != null)
            {
                buttonCanvasGroup.alpha = targetAlpha;
            }
            
            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        
        if (buttonImage != null)
        {
            Color imgColor = Color.white;
            imgColor.a = 1f;
            buttonImage.color = imgColor;
        }
        if (buttonText != null)
        {
            buttonText.text = originalButtonText;
            buttonText.color = Color.white;
            buttonText.alpha = 1f;
        }
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        }
        if (startButton != null)
        {
            startButton.GetComponent<RectTransform>().anchoredPosition = originalButtonPos;
        }
    }
    
    string GetRandomGlitchText(string original)
    {
        if (string.IsNullOrEmpty(original)) return original;
        
        string glitchChars = "!@#$%^&*[]{}<>?/\\|";
        char[] result = original.ToCharArray();
        
        for (int i = 0; i < result.Length; i++)
        {
            if (Random.Range(0, 3) == 0)
            {
                result[i] = glitchChars[Random.Range(0, glitchChars.Length)];
            }
        }
        
        return new string(result);
    }
}