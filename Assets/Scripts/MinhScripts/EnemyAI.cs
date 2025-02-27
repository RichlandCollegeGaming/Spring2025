using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int MaxHealth = 50;
    private int currentHealth;
    public float moveSpeed = 2.0f;
    public Transform target;
    public AudioClip deathSound;
    private bool isDead = false;

    void Start()
    {
        currentHealth = MaxHealth;
        Debug.Log("Enemy started with health: " + currentHealth); // Debug to confirm health initialization
    }

    void Update()
    {
        if (!isDead && target != null)
        {
            FollowTarget();
        }
    }

    void FollowTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        transform.LookAt(target);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage. Current health: " + currentHealth); // Debug to confirm damage applied

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Enemy has died.");

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject, 1.0f); // Delay destruction for effects/animation
    }
}
