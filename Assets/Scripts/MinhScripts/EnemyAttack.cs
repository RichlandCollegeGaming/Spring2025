using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damageAmount = 20f;

    public void Attack(GameObject target)
    {
        if (target.CompareTag("Player"))
        {
            HealthManager healthManager = target.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damageAmount);
            }
        }
    }
}