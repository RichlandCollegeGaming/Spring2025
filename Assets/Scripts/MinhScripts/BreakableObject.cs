using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject breakEffectPrefab; // particle effect
    public float breakForceThreshold = 5f; // Minimum impact force to break

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= breakForceThreshold)
        {
            BreakObject(collision.contacts[0].point);
        }
    }

    private void BreakObject(Vector3 contactPoint)
    {
        // Spawn particle effect
        if (breakEffectPrefab != null)
        {
            GameObject effect = Instantiate(breakEffectPrefab, contactPoint, Quaternion.identity);

            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(effect, 2f);
            }
        }

        // Destroy the breakable object
        Destroy(gameObject);
    }
}
