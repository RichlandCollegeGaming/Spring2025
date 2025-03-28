using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealthEnemy = 100f; // Maximum health value
    private float healthAmount; // Current health

    void Start()
    {
        healthAmount = maxHealthEnemy; // Initialize health at max
        UpdateEnemyHealthUI();
    }

    void Update()
    {
        if (healthAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void EnemyTakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthEnemy); // Prevent health going negative
        UpdateEnemyHealthUI();
    }

    private void UpdateEnemyHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHealthEnemy;
        }
    }
}
