using UnityEngine;

public class PickupItem : MonoBehaviour
{
    bool isHolding = false;
    bool hasBeenThrown = false;

    [SerializeField] float throwForce = 600f;
    [SerializeField] float maxDistance = 3f;
    [SerializeField] GameObject hitParticlePrefab;
    [SerializeField] float damageAmount = 20f;

    float distance;
    TempParent tempParent;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = ~(1 << LayerMask.NameToLayer("WeaponLayer")); // Ignore weapon layer

            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                if (hit.transform == this.transform)
                {
                    if (!isHolding)
                    {
                        isHolding = true;
                        hasBeenThrown = false;
                        rb.useGravity = false;
                        rb.detectCollisions = true;
                        transform.SetParent(tempParent.transform);
                    }
                    else
                    {
                        Drop();
                    }
                }
            }
        }

        if (isHolding)
        {
            Hold();
        }
    }

    private void Hold()
    {
        distance = Vector3.Distance(transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop();
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            isHolding = false;
            hasBeenThrown = true;
            transform.SetParent(null);
            rb.useGravity = true;
            rb.AddForce(tempParent.transform.forward * throwForce);
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            transform.SetParent(null);
            rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenThrown)
        {
            // 1. Try to deal damage
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.EnemyTakeDamage(damageAmount);
                }
            }

            // 2. Spawn hit particle
            if (hitParticlePrefab != null)
            {
                ContactPoint contact = collision.contacts[0];
                GameObject particleInstance = Instantiate(hitParticlePrefab, contact.point, Quaternion.identity);

                ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Play();
                    Destroy(particleInstance, ps.main.duration + ps.main.startLifetime.constantMax);
                }
                else
                {
                    // fallback: destroy after 2 seconds if no ParticleSystem found
                    Destroy(particleInstance, 2f);
                }
            }


            // 3. Destroy this thrown object
            Destroy(gameObject);
        }
    }
}
