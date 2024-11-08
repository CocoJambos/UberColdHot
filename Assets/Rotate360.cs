using UnityEngine;

public class Rotate360 : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Speed of rotation, adjust as needed

    void Update()
    {
        // Rotate the object 360 degrees around its Y-axis
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime * 360);
    }
}
