using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealthEnemy = 100f;
    private float healthAmount;

    private Animator animator; // Reference to the Animator
    private bool isDead = false; // Prevent multiple death triggers

    void Start()
    {
        healthAmount = maxHealthEnemy;
        animator = GetComponentInChildren<Animator>(); // Finds Animator on child object
        UpdateEnemyHealthUI();
    }



   void Update()
{
    if (!isDead && healthAmount <= 0)
    {
        isDead = true;
        animator.SetTrigger("Die"); // Trigger death animation
        GetComponent<NavMeshAgent>().enabled = false; // Disable movement
        GetComponent<Collider>().enabled = false; // Disable collider
        
        // Ensure animation plays fully before disabling the Animator
        StartCoroutine(WaitAndDestroy());
    }
}

IEnumerator WaitAndDestroy()
{
    // Wait for the death animation to finish before destroying the object
    yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    animator.enabled = false; // Disable the Animator to prevent any further animation updates
    Destroy(gameObject); // Destroy the enemy after the animation
}


    public void EnemyTakeDamage(float damage)
    {
        if (isDead) return;

        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, maxHealthEnemy);
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
