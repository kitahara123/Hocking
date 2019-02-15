using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Камера вращающаяся вокруг персонажа
/// </summary>
public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float rotSpeed = 1.5f;
    [SerializeField] private float mouseSens = 3.0f;
    [SerializeField] private int minVertAngles = -40;
    [SerializeField] private int maxVertAngles = 50;
    [SerializeField] private float cameraDistance = 1;
    [SerializeField] private float isometricVertAngle = 35;
    [SerializeField] private float isometricHorAngle = 25;
    [SerializeField] private float isometricDistance = 2;

    private float rotY;
    private float rotX;
    private Vector3 offset;
    public bool Locked { get; private set; } = false;
    private bool isometric;
    private Dictionary<Collider, TmpData> tmpDatas;

    private void Start()
    {
        rotY = transform.eulerAngles.y;
        rotX = transform.eulerAngles.x;
        offset = target.position - transform.position;
        isometric = Managers.Managers.Settings.Isometric;
        Messenger<bool>.AddListener(GameEvent.CAMERA_LOCK, LockUnlock);
        Messenger<bool>.AddListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
        tmpDatas = new Dictionary<Collider, TmpData>();
    }

    protected void OnDestroy() => Messenger<bool>.RemoveListener(GameEvent.ISOMETRIC_ENABLED, OnIsometricEnabled);
    private void OnIsometricEnabled(bool value) => isometric = value;

    private void LateUpdate()
    {
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
        transform.position = target.position - (rotation * (offset * (isometric
                                                                ? isometricDistance
                                                                : cameraDistance)
                                                ));
        transform.LookAt(target);
    }


    // Делает прозрачными объекты перед камерой
    private void OnTriggerEnter(Collider other)
    {
        if (!isometric ||
            other.gameObject.layer == LayerMask.NameToLayer("Chars") ||
            other.gameObject.CompareTag("Device")) return;
        var rend = other.gameObject.GetComponent<MeshRenderer>();
        if (rend == null) return;

        tmpDatas.Add(other, new TmpData(rend.material, other.gameObject.layer));

        var mat = new Material(rend.material);

        mat.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

        mat.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 0.3f);
        rend.material = mat;

        other.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isometric ||
            other.gameObject.layer == LayerMask.NameToLayer("Chars") ||
            other.gameObject.CompareTag("Device")) return;
        var rend = other.gameObject.GetComponent<MeshRenderer>();
        if (rend == null) return;

        rend.material = tmpDatas[other].material;
        other.gameObject.layer = tmpDatas[other].layer;
        tmpDatas.Remove(other);
    }

    public void LockUnlock(bool value) => Locked = value;

    private class TmpData
    {
        public Material material;
        public LayerMask layer;

        public TmpData(Material material, LayerMask layer)
        {
            this.material = material;
            this.layer = layer;
        }
    }
}