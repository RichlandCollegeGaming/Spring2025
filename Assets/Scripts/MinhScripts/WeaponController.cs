using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword;
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f;
    public float ChargeAttackCooldown = 2.5f;
    public float ChargeTime = 2.0f;
    public AudioClip SwordAttackSound;
    public AudioClip ChargedAttackSound;
    public bool IsAttacking = false;
    private bool isCharging = false;
    private float chargeTimer = 0f;
    public PlayerHealth playerHealth;

    // Damage variables for normal and charged attacks
    public int NormalAttackDamage = 10;
    public int ChargedAttackDamage = 20;

    public bool IsCharging()
    {
        return isCharging;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            StartCoroutine(ChargeAttack());
        }
        if (Input.GetMouseButtonUp(0) && isCharging)
        {
            CancelCharge();
        }
    }

    public void SwordAttack()
    {
        IsAttacking = true;
        CanAttack = false;
        Animator anim = Sword.GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("Attack");
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(SwordAttackSound);
        }
        else
        {
            Debug.LogError("No Animator component found on Sword GameObject.");
        }

        StartCoroutine(ResetAttackCooldown(AttackCooldown));
    }

    private IEnumerator ChargeAttack()
    {
        isCharging = true;
        chargeTimer = 0f;

        while (chargeTimer < ChargeTime)
        {
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        if (isCharging) // If still holding after full charge time, perform charge attack
        {
            PerformChargeAttack();
        }
    }

    private void PerformChargeAttack()
    {
        isCharging = false;
        CanAttack = false;
        Animator anim = Sword.GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("ChargeAttack");
            AudioSource ac = GetComponent<AudioSource>();
            ac.PlayOneShot(ChargedAttackSound);
        }
        else
        {
            Debug.LogError("No Animator component found on Sword GameObject.");
        }

        StartCoroutine(ResetAttackCooldown(ChargeAttackCooldown));
    }

    private void CancelCharge()
    {
        if (isCharging)
        {
            isCharging = false;
            StopCoroutine(ChargeAttack()); // Stops charging if released early
            SwordAttack();
        }
    }

    // Apply damage to the enemy if it is in the attack range
    public void ApplyDamageToEnemy(Collider enemyCollider, int damage)
    {
        // Find the EnemyAI script on the enemy
        EnemyAI enemyAI = enemyCollider.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            // Call the TakeDamage method on the enemy
            enemyAI.TakeDamage(damage);
        }
    }

    IEnumerator ResetAttackCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        CanAttack = true;
        IsAttacking = false; // Ensure attack state is properly reset
    }
}
