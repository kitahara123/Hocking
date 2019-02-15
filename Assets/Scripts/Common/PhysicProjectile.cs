using UnityEngine;

/// <summary>
/// Класс для снарядов с поведением основанным на физике
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PhysicProjectile : Projectile
{
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * baseSpeed);
    }

    protected override void Update()
    {
    }
}