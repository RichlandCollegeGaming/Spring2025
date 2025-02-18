using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword;
    public bool CanAttack = true;
    public float AttackCooldown = 1.0f;
    public AudioClip SwordAttackSound;
    public bool IsAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanAttack)
        {
            SwordAttack();
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

        StartCoroutine(ResetAttackCooldown());
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBool());
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }
    IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(1.0f);
        IsAttacking = false;
    }


}
