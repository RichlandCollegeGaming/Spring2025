using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword;
    public GameObject HitParticle;
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
            GetComponent<AudioSource>().PlayOneShot(SwordAttackSound);
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

        if (isCharging)
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
            GetComponent<AudioSource>().PlayOneShot(ChargedAttackSound);
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
            StopCoroutine(ChargeAttack());
            SwordAttack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && IsAttacking)
        {
            Debug.Log("Enemy Hit! Spawning Blood Effect.");

            Animator enemyAnim = other.GetComponent<Animator>();
            if (enemyAnim != null)
            {
                enemyAnim.SetTrigger("Hit");
            }

            int damage = isCharging ? ChargedAttackDamage : NormalAttackDamage;
            ApplyDamageToEnemy(other, damage);

            Vector3 spawnPosition = other.transform.position + Vector3.up * 1.5f; // Adjust height for visibility
            GameObject particleInstance = Instantiate(HitParticle, spawnPosition, Quaternion.identity);

            if (particleInstance != null)
            {
                ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play(); // Ensure the particle system plays
                }
            }

            Destroy(particleInstance, 2.0f); // Extended duration for visibility
        }
    }

    public void ApplyDamageToEnemy(Collider enemyCollider, int damage)
    {
        EnemyAI enemyAI = enemyCollider.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(damage);
        }
    }

    private IEnumerator ResetAttackCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        CanAttack = true;
        IsAttacking = false;
    }
}
