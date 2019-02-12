using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : SpeedControl
{
    [SerializeField] private int damage = 1;
    public Creature Shooter { get; set; }

    protected virtual void Start()
    {
        Destroy(gameObject, 10);
    }

    protected virtual void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var creature = other.GetComponent<Creature>();
        if (creature == null)
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag(Shooter.tag) || other is SphereCollider) return;
        creature.Hurt(damage);
        Destroy(gameObject);
    }
}