using UnityEngine;

public class PickupItem : MonoBehaviour
{
    bool isHolding = false;

    [SerializeField]
    float throwForce = 600f;
    [SerializeField]
    float maxDistance = 3f;
    float distance;

    TempParent tempParent;
    Rigidbody rb;

    Vector3 objectPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tempParent = TempParent.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int layerMask = ~(1 << LayerMask.NameToLayer("WeaponLayer")); // ignore weapon layer

            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                if (hit.transform == this.transform)
                {
                    if (!isHolding)
                    {
                        isHolding = true;
                        rb.useGravity = false;
                        rb.detectCollisions = true;

                        this.transform.SetParent(tempParent.transform);
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


    // private void OnMouseUp()
    //  {
    // //     Drop();
    // }
    private void OnMouseExit()
    {
        Drop();
    }
    
    private void Hold()
    {
        distance = Vector3.Distance(this.transform.position, tempParent.transform.position);

        if (distance >= maxDistance)
        {
            Drop();
        }

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (Input.GetMouseButtonDown(1))
        {
            rb.AddForce(tempParent.transform.forward * throwForce);
            Drop();
        }
    }

    private void Drop()
    {
        if (isHolding)
        {
            isHolding = false;
            objectPos = this.transform.position;
            this.transform.position = objectPos;
            this.transform.SetParent(null);
            rb.useGravity = true;
        }
    }
}   
