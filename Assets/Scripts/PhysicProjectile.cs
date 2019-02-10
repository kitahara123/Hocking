
using UnityEngine;

public class PhysicProjectile : Projectile
{
    protected override void Start()
    {
        base.Start();
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    protected override void Update()
    {
        
    }
}