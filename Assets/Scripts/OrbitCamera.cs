using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float rotSpeed = 1.5f;
    [SerializeField] private float mouseSens = 3.0f;
    [SerializeField] private int minVertAngles = -40;
    [SerializeField] private int maxVertAngles = 50;
    [SerializeField] private float cameraDistanse = 1;
    [SerializeField] private float isometricVertAngle = 35;
    [SerializeField] private float isometricHorAngle = 25;
    [SerializeField] private float isometricDistanse = 2;

    private float rotY;
    private float rotX;
    private Vector3 offset;
    public bool Locked { get; private set; } = false;

    private void Start()
    {
        rotY = transform.eulerAngles.y;
        rotX = transform.eulerAngles.x;
        offset = target.position - transform.position;
        Messenger<bool>.AddListener(GameEvent.CAMERA_LOCK, LockUnlock);
    }

    private void LateUpdate()
    {
        var isometric = Managers.Managers.Settings.Isometric;

        if (!Locked && !isometric)
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * mouseSens;
            rotX += Input.GetAxis("Mouse Y") * rotSpeed * mouseSens * -1;
        }

        if (isometric)
        {
            rotY = isometricVertAngle;
            rotX = isometricHorAngle;
        }

        rotX = Mathf.Clamp(rotX, minVertAngles, maxVertAngles);

        var rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.position = target.position -
                             (rotation * (offset * (isometric
                                              ? isometricDistanse
                                              : cameraDistanse)
                              ));
        transform.LookAt(target);
    }

    public void LockUnlock(bool value)
    {
        Locked = value;
    }
}