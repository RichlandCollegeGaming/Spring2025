using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public WeaponController wc; // Reference to the WeaponController
    public GameObject HitParticle;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && wc.IsAttacking)
        {
            Debug.Log(other.name);

            // Trigger the Hit animation on the enemy
            Animator enemyAnim = other.GetComponent<Animator>();
            if (enemyAnim != null)
            {
                enemyAnim.SetTrigger("Hit");
            }

            // Apply damage to the enemy: Check if it's a charged attack or normal attack
            int damage = wc.IsCharging() ? wc.ChargedAttackDamage : wc.NormalAttackDamage;
            wc.ApplyDamageToEnemy(other, damage);

            // Instantiate hit particles
            GameObject particleInstance = Instantiate(HitParticle, new Vector3(other.transform.position.x,
            transform.position.y, other.transform.position.z),
            other.transform.rotation);

            Destroy(particleInstance, 1.0f);
        }
    }
}