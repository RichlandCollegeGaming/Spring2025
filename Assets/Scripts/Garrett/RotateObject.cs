using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Rotation values in degrees
    public float rotationX = 0f;
    public float rotationY = 0f;
    public float rotationZ = 0f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object by the set values in X, Y, and Z
        transform.Rotate(rotationX * Time.deltaTime, rotationY * Time.deltaTime, rotationZ * Time.deltaTime);
    }
}
