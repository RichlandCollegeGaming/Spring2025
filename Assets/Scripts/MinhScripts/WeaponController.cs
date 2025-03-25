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

    // Damage values
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
            Debug.Log("Normal Attack triggered.");
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
        Debug.Log("Charging attack...");

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
            Debug.Log("Charged Attack performed!");
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
            Debug.Log("Charge attack canceled. Performing normal attack.");
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

            Vector3 spawnPosition = other.transform.position + Vector3.up * 1.5f;
            GameObject particleInstance = Instantiate(HitParticle, spawnPosition, Quaternion.identity);

            if (particleInstance != null)
            {
                ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                }
            }

            Destroy(particleInstance, 2.0f);
        }
    }

    public void ApplyDamageToEnemy(Collider enemyCollider, int damage)
    {
        EnemyAI enemyAI = enemyCollider.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            enemyAI.TakeDamage(damage);
            Debug.Log($"Enemy took {damage} damage.");
        }
    }

    private IEnumerator ResetAttackCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        CanAttack = true;
        IsAttacking = false;
        Debug.Log("Attack cooldown reset.");
    }
}
