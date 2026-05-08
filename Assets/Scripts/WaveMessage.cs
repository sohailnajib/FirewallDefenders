using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class WaveMessage : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject messagePanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button skipButton;
    public Image panelBackground;

    [Header("Background Panel")]
    public Image darkBackgroundPanel;

    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;
    public float glitchDuration = 0.3f;
    public float scalePunchDuration = 0.2f;

    [Header("Colors")]
    public Color titleColor = new Color(0f, 0.8f, 1f);
    public Color descriptionColor = new Color(0.8f, 0.8f, 1f);

    [Header("Dark Background Settings")]
    public Color backgroundColor = new Color(0.05f, 0.05f, 0.1f, 0.92f);
    public Color borderColor = new Color(0f, 1f, 0.8f, 0.5f);
    public float borderWidth = 2f;

    [Header("Glitch Settings")]
    public float glitchIntensity = 0.1f;

    [Header("Skip Button Settings")]
    public Vector2 buttonSize = new Vector2(200f, 50f);
    public Vector2 buttonHoverSize = new Vector2(250f, 55f);
    public float buttonFontSize = 28f;
    public float buttonHoverFontSize = 32f;
    public Color buttonNormalColor = new Color(0.1f, 0.15f, 0.25f, 0.9f);
    public Color buttonHoverColor = Color.white;
    public string buttonNormalText = "SKIP";
    public string buttonHoverText = "SKIP NOW";

    private bool skipped = false;
    private Vector2 originalTitlePos;
    private Vector2 originalDescPos;
    private CanvasGroup canvasGroup;

    private string[] waveTitles = {
        "WAVE 1 — VIRUS DETECTED!",
        "WAVE 2 — DDoS ATTACK!",
        "WAVE 3 — WORM OUTBREAK!",
        "WAVE 4 — TROJAN HORSE!",
        "WAVE 5 — RANSOMWARE ALERT!",
        "INCOMING THREAT!"
    };

    private string[] waveDescriptions = {
        "<color=#FF4444>⚠ CRITICAL ALERT ⚠</color>\n\nViruses attach to programs and replicate,\ncorrupting data and slowing systems.\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Destroy all viruses before\nthey reach and corrupt your Firewall!",

        "<color=#FF8800>⚠ DDoS ATTACK DETECTED ⚠</color>\n\nDistributed Denial of Service floods your\nfirewall with fake requests to overwhelm it.\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Eliminate the DDoS bots before\nthey crash your Firewall!",

        "<color=#FF4444>⚠ WORM OUTBREAK ⚠</color>\n\nUnlike viruses, worms self-replicate across\nnetworks without needing a host program.\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Stop the worms from spreading\nthrough your Firewall!",

        "<color=#FF8800>⚠ TROJAN HORSE ⚠</color>\n\nMalware disguised as legitimate software\nthat opens backdoors for attackers.\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Destroy the Trojans before\nthey breach your Firewall!",

        "<color=#FF4444>⚠ RANSOMWARE ALERT ⚠</color>\n\nMalware that encrypts your data and\ndemands payment to restore access.\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Eliminate all Ransomware before\nit encrypts your Firewall!",

        "<color=#FF00FF>⚠ UNKNOWN THREAT ⚠</color>\n\nUnknown threat detected!\n\n<color=#00FFAA>▸ OBJECTIVE:</color> Destroy all threats before\nthey reach your Firewall!"
    };

    void Start()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);

            canvasGroup = messagePanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = messagePanel.AddComponent<CanvasGroup>();

            
            RectTransform panelRect = messagePanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                panelRect.anchorMin = Vector2.zero;
                panelRect.anchorMax = Vector2.one;
                panelRect.offsetMin = Vector2.zero;
                panelRect.offsetMax = Vector2.zero;
            }
            
        }

        SetupDarkBackground();

        if (skipButton != null)
            skipButton.onClick.AddListener(SkipMessage);

        if (titleText != null)
            originalTitlePos = titleText.rectTransform.anchoredPosition;

        if (descriptionText != null)
            originalDescPos = descriptionText.rectTransform.anchoredPosition;

        StyleSkipButton();
    }

    void SetupDarkBackground()
    {
        if (darkBackgroundPanel == null && messagePanel != null)
        {
            GameObject bgObj = new GameObject("DarkBackground");
            bgObj.transform.SetParent(messagePanel.transform, false);
            bgObj.transform.SetAsFirstSibling();

            darkBackgroundPanel = bgObj.AddComponent<Image>();

            RectTransform rect = darkBackgroundPanel.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        if (darkBackgroundPanel != null)
        {
            darkBackgroundPanel.color = backgroundColor;

            Outline outline = darkBackgroundPanel.GetComponent<Outline>();
            if (outline == null)
                outline = darkBackgroundPanel.gameObject.AddComponent<Outline>();

            outline.effectColor = borderColor;
            outline.effectDistance = new Vector2(borderWidth, borderWidth);
        }
    }

    void StyleSkipButton()
    {
        if (skipButton == null) return;

        RectTransform buttonRect = skipButton.GetComponent<RectTransform>();
        if (buttonRect != null)
            buttonRect.sizeDelta = buttonSize;

        Image btnImage = skipButton.GetComponent<Image>();
        if (btnImage == null)
            btnImage = skipButton.gameObject.AddComponent<Image>();

        btnImage.color = buttonNormalColor;
        btnImage.raycastTarget = true;

        TextMeshProUGUI btnText = skipButton.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.text = buttonNormalText;
            btnText.color = buttonHoverColor;
            btnText.fontSize = buttonFontSize;
            btnText.fontStyle = FontStyles.Bold;
            btnText.alignment = TextAlignmentOptions.Center;
        }

        SkipButtonHoverEffect hoverEffect = skipButton.GetComponent<SkipButtonHoverEffect>();
        if (hoverEffect == null)
            hoverEffect = skipButton.gameObject.AddComponent<SkipButtonHoverEffect>();

        hoverEffect.SetButtonData(buttonSize, buttonHoverSize, buttonFontSize, buttonHoverFontSize,
                                  buttonNormalColor, buttonHoverColor, buttonNormalText, buttonHoverText);
    }

    public void SkipMessage()
    {
        skipped = true;
    }

    public IEnumerator ShowWaveMessage(int waveNumber)
    {
        skipped = false;

        int index = Mathf.Clamp(waveNumber - 1, 0, waveTitles.Length - 1);

        if (titleText != null)
        {
            titleText.text = waveTitles[index];
            titleText.color = titleColor;
            titleText.fontStyle = FontStyles.Bold;
        }

        if (descriptionText != null)
        {
            descriptionText.text = waveDescriptions[index];
            descriptionText.color = descriptionColor;
        }

        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
            StartCoroutine(PanelPulseAnimation());
        }

        StartCoroutine(GlitchEntrance());
        StartCoroutine(FadeInPanel());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float autoSkipTime = 8f;
        float timer = 0;

        while (!skipped && timer < autoSkipTime)
        {
            timer += Time.deltaTime;

            if (titleText != null && Random.Range(0f, 1f) < 0.05f)
                StartCoroutine(TitleGlitch());

            yield return null;
        }

        yield return StartCoroutine(FadeOutPanel());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    IEnumerator GlitchEntrance()
    {
        float elapsed = 0;
        while (elapsed < glitchDuration)
        {
            if (titleText != null)
            {
                float offsetX = Random.Range(-glitchIntensity, glitchIntensity);
                float offsetY = Random.Range(-glitchIntensity * 0.5f, glitchIntensity * 0.5f);
                titleText.rectTransform.anchoredPosition = new Vector2(
                    originalTitlePos.x + offsetX,
                    originalTitlePos.y + offsetY
                );

                if (Random.Range(0f, 1f) < 0.3f)
                    titleText.color = new Color(Random.Range(0.5f, 1f), Random.Range(0f, 0.5f), Random.Range(0f, 0.5f));
                else
                    titleText.color = titleColor;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (titleText != null)
        {
            titleText.rectTransform.anchoredPosition = originalTitlePos;
            titleText.color = titleColor;
        }
    }

    IEnumerator TitleGlitch()
    {
        Vector2 originalPos = titleText.rectTransform.anchoredPosition;
        Color originalCol = titleText.color;

        titleText.rectTransform.anchoredPosition = new Vector2(
            originalPos.x + Random.Range(-5f, 5f),
            originalPos.y + Random.Range(-2f, 2f)
        );
        titleText.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        titleText.rectTransform.anchoredPosition = new Vector2(
            originalPos.x + Random.Range(-3f, 3f),
            originalPos.y + Random.Range(-1f, 1f)
        );
        titleText.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        titleText.rectTransform.anchoredPosition = originalPos;
        titleText.color = originalCol;
    }

    IEnumerator FadeInPanel()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            float elapsed = 0;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeInDuration);
                yield return null;
            }
            canvasGroup.alpha = 1;
        }
    }

    IEnumerator FadeOutPanel()
    {
        if (canvasGroup != null)
        {
            float elapsed = 0;
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeInDuration);
                yield return null;
            }
            canvasGroup.alpha = 0;
        }
    }

    IEnumerator PanelPulseAnimation()
    {
        if (panelBackground != null)
        {
            Vector3 originalScale = panelBackground.rectTransform.localScale;
            float elapsed = 0;
            while (elapsed < scalePunchDuration)
            {
                elapsed += Time.deltaTime;
                float t = 1 - (elapsed / scalePunchDuration);
                float scale = 1 + (t * 0.05f);
                panelBackground.rectTransform.localScale = new Vector3(scale, scale, 1);
                yield return null;
            }
            panelBackground.rectTransform.localScale = originalScale;
        }
    }
}

[System.Serializable]
public class SkipButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private TextMeshProUGUI buttonText;
    private Image buttonImage;
    private RectTransform buttonRect;

    private Vector2 originalSize;
    private Vector2 hoverSize;
    private float originalFontSize;
    private float hoverFontSize;
    private Color originalColor;
    private Color hoverColor;
    private string originalText;
    private string hoverText;

    public void SetButtonData(Vector2 origSize, Vector2 hoverSz, float origFont, float hoverFont,
                              Color origColor, Color hoverCol, string origTxt, string hoverTxt)
    {
        originalSize = origSize;
        hoverSize = hoverSz;
        originalFontSize = origFont;
        hoverFontSize = hoverFont;
        originalColor = origColor;
        hoverColor = hoverCol;
        originalText = origTxt;
        hoverText = hoverTxt;
    }

    void Start()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonImage = GetComponent<Image>();
        buttonRect = GetComponent<RectTransform>();

        if (buttonRect != null)
            originalSize = buttonRect.sizeDelta;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button != null && button.interactable)
        {
            if (buttonText != null)
            {
                buttonText.text = hoverText;
                buttonText.fontSize = hoverFontSize;
                buttonText.color = hoverColor;
            }

            if (buttonRect != null)
                buttonRect.sizeDelta = hoverSize;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.text = originalText;
            buttonText.fontSize = originalFontSize;
            buttonText.color = hoverColor;
        }

        if (buttonRect != null)
            buttonRect.sizeDelta = originalSize;
    }
}
