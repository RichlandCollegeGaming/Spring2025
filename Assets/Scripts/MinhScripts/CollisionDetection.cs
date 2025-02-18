using UnityEngine;
using static UnityEngine.ParticleSystem;

public class CollisionDetection : MonoBehaviour
{
    public WeaponController wc;
    public GameObject HitParticle;



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && wc.IsAttacking)
        {
            Debug.Log(other.name);
            other.GetComponent<Animator>().SetTrigger("Hit");
            Instantiate(HitParticle, new Vector3(other.transform.position.x,
            transform.position.y, other.transform.position.z),
            other.transform.rotation);
         
        }
    }
}
