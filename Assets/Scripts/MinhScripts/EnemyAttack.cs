using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damageAmount = 20f; // Amount to restore

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player
        {
            HealthManager healthManager = other.GetComponent<HealthManager>(); // Get HealthManager from the player
            if (healthManager != null)
            {
                healthManager.TakeDamage(damageAmount); // Damage the player (negative damage)
                Destroy(gameObject); // Destroy the pickup
            }
        }
    }
}
