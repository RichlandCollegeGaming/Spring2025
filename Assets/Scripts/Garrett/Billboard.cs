using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        // Get the direction vector from the object to the camera
        Vector3 directionToCamera = Camera.main.transform.position - transform.position;

        // Set the rotation of the object to face the camera, including the X, Y, and Z axes
        transform.rotation = Quaternion.LookRotation(directionToCamera);
    }
}
