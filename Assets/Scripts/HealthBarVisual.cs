using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarVisual : MonoBehaviour
{
    [Header("Health Bar Setup")]
    public Image healthBarImage;
    public FirewallHealth firewallHealth;

    [Header("Color Settings")]
    public Gradient healthGradient;

    [Header("Pulse Settings (Low Health)")]
    public bool enablePulseEffect = true;
    public float pulseSpeed = 3f;
    public float pulseScaleIntensity = 0.2f;
    public float pulseAlphaIntensity = 0.3f;
    public float lowHealthThreshold = 0.33f;

    private float pulseTimer;
    private Vector3 originalScale;
    private bool wasLowHealth = false;

    void Start()
    {
        if (firewallHealth == null)
            firewallHealth = FindObjectOfType<FirewallHealth>();

        if (healthBarImage != null)
            originalScale = healthBarImage.rectTransform.localScale;

        SetupGradient();
        UpdateHealthBar();
    }

    void SetupGradient()
    {
        healthGradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[4];
        colorKeys[0] = new GradientColorKey(Color.green, 1f);
        colorKeys[1] = new GradientColorKey(Color.yellow, 0.66f);
        colorKeys[2] = new GradientColorKey(new Color(1f, 0.5f, 0f), 0.33f);
        colorKeys[3] = new GradientColorKey(Color.red, 0f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 0f);
        alphaKeys[1] = new GradientAlphaKey(1f, 1f);

        healthGradient.SetKeys(colorKeys, alphaKeys);
    }

    void Update()
    {
        if (firewallHealth != null && healthBarImage != null)
            UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        float healthPercent = (float)firewallHealth.GetHealth() / (float)firewallHealth.GetMaxHealth();

        if (healthBarImage.type == Image.Type.Filled)
            healthBarImage.fillAmount = healthPercent;

        Color targetColor = healthGradient.Evaluate(healthPercent);
        bool isLowHealth = healthPercent <= lowHealthThreshold;

        if (isLowHealth && enablePulseEffect)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            float pulseValue = (Mathf.Sin(pulseTimer) + 1f) / 2f;

            float scaleMultiplier = 1f + (pulseValue * pulseScaleIntensity);
            healthBarImage.rectTransform.localScale = new Vector3(
                originalScale.x * scaleMultiplier,
                originalScale.y * scaleMultiplier,
                originalScale.z
            );

            targetColor.a = 0.7f + (pulseValue * pulseAlphaIntensity);

            // Flash deep red when critically low
            if (healthPercent < 0.15f)
                targetColor = Color.Lerp(Color.red, new Color(0.6f, 0f, 0f), pulseValue);

            wasLowHealth = true;
        }
        else
        {
            if (wasLowHealth)
            {
                healthBarImage.rectTransform.localScale = originalScale;
                pulseTimer = 0;
                wasLowHealth = false;
            }
            targetColor.a = 1f;
        }

        healthBarImage.color = targetColor;
    }

    public void FlashOnDamage()
    {
        if (healthBarImage != null)
            StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        Color original = healthBarImage.color;
        healthBarImage.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        healthBarImage.color = original;
    }
}
