
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicProjectile : Projectile
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * baseSpeed);
    }

    protected override void Update()
    {
    }
}