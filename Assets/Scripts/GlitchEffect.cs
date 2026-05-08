using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GlitchEffects : MonoBehaviour
{
    [Header("Background")]
    public Image backgroundImage;

    [Header("Text Elements")]
    public TMP_Text[] glitchTexts;

    [Header("Start Button")]
    public Button startButton;
    public Image buttonImage;
    public TMP_Text buttonText;

    [Header("Timing")]
    public float bgGlitchDuration = 2f;
    public float textGlitchDuration = 1.5f;
    public float buttonGlitchDuration = 1f;
    private Vector2 originalBgPos;
    private Color originalBgColor;
    private string[] originalTexts;
    private Color[] originalTextColors;

    void Start()
    {
        if (backgroundImage != null)
        {
            originalBgPos = backgroundImage.rectTransform.anchoredPosition;
            originalBgColor = backgroundImage.color;
        }

        if (glitchTexts != null && glitchTexts.Length > 0)
        {
            originalTexts = new string[glitchTexts.Length];
            originalTextColors = new Color[glitchTexts.Length];

            for (int i = 0; i < glitchTexts.Length; i++)
            {
                if (glitchTexts[i] != null)
                {
                    originalTexts[i] = glitchTexts[i].text;
                    originalTextColors[i] = glitchTexts[i].color;
                    glitchTexts[i].alpha = 0f;
                }
            }
        }

        if (startButton != null)
        {
            CanvasGroup buttonCanvasGroup = startButton.GetComponent<CanvasGroup>();
            if (buttonCanvasGroup == null)
                buttonCanvasGroup = startButton.gameObject.AddComponent<CanvasGroup>();

            buttonCanvasGroup.alpha = 0f;
            buttonCanvasGroup.interactable = false;
            buttonCanvasGroup.blocksRaycasts = false;
            startButton.interactable = false;
        }

        if (buttonText != null)
            buttonText.alpha = 0f;

        if (buttonImage != null)
        {
            Color c = buttonImage.color;
            c.a = 0f;
            buttonImage.color = c;
        }

        StartCoroutine(FullGlitchSequence());
    }

    IEnumerator FullGlitchSequence()
    {
        yield return StartCoroutine(GlitchBackground());
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(GlitchText());
        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(GlitchButton());

        if (startButton != null)
            startButton.interactable = true;
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

        while (elapsed < textGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);

            for (int i = 0; i < glitchTexts.Length; i++)
            {
                TMP_Text textElement = glitchTexts[i];
                if (textElement == null) continue;

                float targetAlpha = Mathf.Lerp(0f, 1f, elapsed / textGlitchDuration);

                if (intensity > 0.6f)
                {
                    if (originalTexts != null && i < originalTexts.Length)
                        textElement.text = GetRandomGlitchText(originalTexts[i]);

                    textElement.color = new Color(
                        Random.Range(0.8f, 1f),
                        Random.Range(0f, 0.5f),
                        Random.Range(0f, 0.5f),
                        1
                    );
                    textElement.alpha = targetAlpha;
                }
                else if (intensity > 0.3f)
                {
                    textElement.color = new Color(0.7f, 0.5f, 1f, 1f);
                    textElement.alpha = targetAlpha;
                }
                else
                {
                    if (originalTexts != null && i < originalTexts.Length)
                        textElement.text = originalTexts[i];
                    textElement.color = originalTextColors[i];
                    textElement.alpha = targetAlpha;
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
                glitchTexts[i].alpha = 1f;
            }
        }
    }

    IEnumerator GlitchButton()
    {
        float elapsed = 0;
        string originalButtonText = buttonText != null ? buttonText.text : "";

        if (buttonText != null)
            buttonText.alpha = 0f;

        CanvasGroup buttonCanvasGroup = startButton != null ? startButton.GetComponent<CanvasGroup>() : null;

        while (elapsed < buttonGlitchDuration)
        {
            float intensity = Random.Range(0f, 1f);
            float targetAlpha = Mathf.Lerp(0f, 1f, elapsed / buttonGlitchDuration);

            if (buttonImage != null)
            {
                buttonImage.color = intensity > 0.5f
                    ? new Color(Random.Range(0.8f, 1f), Random.Range(0f, 0.3f), Random.Range(0f, 0.3f), targetAlpha)
                    : new Color(1, 1, 1, targetAlpha);
            }

            if (buttonText != null)
            {
                if (intensity > 0.7f)
                {
                    buttonText.text = GetRandomGlitchText(originalButtonText);
                    buttonText.color = Color.red;
                }
                else if (intensity > 0.3f)
                {
                    buttonText.color = new Color(1f, 0.5f, 0.5f, 1);
                }
                else
                {
                    buttonText.text = originalButtonText;
                    buttonText.color = Color.white;
                }
                buttonText.alpha = targetAlpha;
            }

            if (buttonCanvasGroup != null)
                buttonCanvasGroup.alpha = targetAlpha;

            elapsed += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        if (buttonImage != null)
            buttonImage.color = new Color(1, 1, 1, 1f);

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

    }

    string GetRandomGlitchText(string original)
    {
        if (string.IsNullOrEmpty(original)) return original;

        string glitchChars = "!@#$%^&*[]{}<>?/\\|";
        char[] result = original.ToCharArray();

        for (int i = 0; i < result.Length; i++)
        {
            if (Random.Range(0, 3) == 0)
                result[i] = glitchChars[Random.Range(0, glitchChars.Length)];
        }

        return new string(result);
    }
}