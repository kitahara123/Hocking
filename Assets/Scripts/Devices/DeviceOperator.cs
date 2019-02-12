using UnityEngine;

public class DeviceOperator : MonoBehaviour
{
    [SerializeField] private float radius = 1.5f;

    private void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (var hitCollider in hitColliders)
            {
                var direction = hitCollider.transform.position - transform.position;
                if (Vector3.Dot(transform.forward, direction) > .5f)
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}