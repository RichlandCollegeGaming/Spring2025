using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healthAmount = 20f; // Amount to restore

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure it's the player
        {
            HealthManager healthManager = other.GetComponent<HealthManager>(); // Get HealthManager from the player
            if (healthManager != null)
            {
                healthManager.TakeDamage(-healthAmount); // Heal the player (negative damage)
                Destroy(gameObject); // Destroy the pickup
            }
        }
    }
}
