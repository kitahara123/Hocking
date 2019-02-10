using UnityEngine;
using UnityEngine.EventSystems;

public class PhysicShooter : SpeedControl
{
    private new Camera camera;
    [SerializeField] private Projectile projectile;
    [SerializeField] private string friendlyFire = "Player";

    private void Start()
    {
        camera = GetComponent<Camera>();

    }

    private void OnGUI()
    {
        var size = 12;
        var posX = camera.pixelWidth / 2 - size / 4;
        var posY = camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    private void Update()
    {
        if (speed > 0 && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var go = Instantiate(projectile);

            go.FriendlyFire = friendlyFire;
            go.transform.rotation = Quaternion.LookRotation(transform.forward);
            go.transform.position = transform.TransformPoint(Vector3.forward * 1);
        }
    }
}