using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float rotSpeed = 1.5f;
    [SerializeField] private float mouseSens = 3.0f;

    private float rotY;
    private Vector3 offset;

    private void Start()
    {
        rotY = transform.eulerAngles.y;
        offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        var horInput = Input.GetAxis("Horizontal");
        if (horInput != 0)
        {
            rotY += horInput * rotSpeed;
        }
        else
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * mouseSens;
        }

        var rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }
}