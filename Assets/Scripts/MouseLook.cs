using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private void Start()
    {
        var body = GetComponent<Rigidbody>();
        if (body == null) return;
        body.freezeRotation = true;
    }


    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    [SerializeField] private RotationAxes axes = RotationAxes.MouseXAndY;
    [SerializeField] private float sensHor = 9.0f;
    [SerializeField] private float sensVert = 9.0f;
    
    [SerializeField] private float minVert = -45.0f;
    [SerializeField] private float maxVert = 45.0f;

    private float rotationX = 0;

    private void Update()
    {
        switch (axes)
        {
            case RotationAxes.MouseX:
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensHor, 0);
                break;
            case RotationAxes.MouseY:
            {
                rotationX -= Input.GetAxis("Mouse Y") * sensVert;

                rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

                float rotationY = transform.localEulerAngles.y;
            
                transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
                break;
            }
            default:
            {
                rotationX -= Input.GetAxis("Mouse Y") * sensVert;

                rotationX = Mathf.Clamp(rotationX, minVert, maxVert);

                float delta = Input.GetAxis("Mouse X") * sensHor;
                float rotationY = transform.localEulerAngles.y + delta;

                transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
                break;
            }
        }
    }
}
