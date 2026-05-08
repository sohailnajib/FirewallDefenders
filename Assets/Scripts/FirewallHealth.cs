using UnityEngine;

public class FirewallHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        HealthBarVisual healthBar = FindObjectOfType<HealthBarVisual>();
        if (healthBar != null)
            healthBar.FlashOnDamage();

        UpdateHealthUI();

        if (currentHealth <= 0)
            GameManager.instance.TriggerGameOver();
    }

    void UpdateHealthUI()
    {
        if (GameManager.instance != null)
            GameManager.instance.UpdateHealth(currentHealth, maxHealth);
    }

    public int GetHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
}
