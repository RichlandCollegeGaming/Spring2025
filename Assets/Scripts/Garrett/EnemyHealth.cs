using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealthEnemy = 100f;
    private float healthAmount;
    private Animator animator;
    private bool isDead = false;
    public AudioSource audioSource;
    public AudioClip[] hitSounds;
    public AudioClip[] deathSounds;

    void Start()
    {
        healthAmount = maxHealthEnemy;
        UpdateEnemyHealthUI();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (!isDead && healthAmount <= 0)
        {
            isDead = true;
            animator.SetTrigger("Die"); // Trigger the death animation
            GetComponent<NavMeshAgent>().enabled = false; // Stop movement
            GetComponent<Collider>().enabled = false; // Disable the collider
            PlayDeathSound();
            // Start coroutine to wait for the death animation to finish
            StartCoroutine(WaitAndDestroy());
        }
    }

    IEnumerator WaitAndDestroy()
    {
        // Wait for the death animation to finish before destroying the object
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false; // Disable the Animator
        Destroy(gameObject); // Destroy the enemy after the animation is done
    }

    public void EnemyTakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthEnemy);
        UpdateEnemyHealthUI();

        PlayHitSound(); // Play sound when hit
        animator.ResetTrigger("Get Hit");
        animator.SetTrigger("Get Hit");
    }

    void PlayHitSound()
    {
        if (audioSource != null && hitSounds.Length > 0)
        {
            int index = Random.Range(0, hitSounds.Length);
            audioSource.PlayOneShot(hitSounds[index]);
        }
    }

    void PlayDeathSound()
    {
        if (audioSource != null && deathSounds.Length > 0)
        {
            int index = Random.Range(0, deathSounds.Length);
            audioSource.PlayOneShot(deathSounds[index]);
        }
    }

    void UpdateEnemyHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = healthAmount / maxHealthEnemy;
        }
    }
}
