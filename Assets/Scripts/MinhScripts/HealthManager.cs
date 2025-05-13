using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 100f; // Maximum health value
    private float healthAmount; // Current health
    public gameoverscreen gameoverscreen;

    void Start()
    {
        healthAmount = maxHealth; // Initialize health at max
        UpdateHealthUI();
    }

    private bool isDead = false;
    void Update()
    {
        if (healthAmount <= 0 && !isDead)
        {
            isDead = true;
            gameoverscreen.Setup();
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); // Prevent health going negative
        UpdateHealthUI();
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealth); // Ensure health doesn't exceed max
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHealth;
        }
    }
}
