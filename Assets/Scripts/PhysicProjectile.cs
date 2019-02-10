
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicProjectile : Projectile
{
    private Rigidbody rigidbody;
    protected override void Start()
    {
        base.Start();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * baseSpeed);
    }

    protected override void Update()
    {
    }
}