using System;
using UnityEngine;

public class Projectile : SpeedControl
{
    [SerializeField] private int damage = 1;
    public Creature Shooter { get; set; }
    public event Action<Projectile> OnDestroyed;

    protected virtual void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;
        var creature = other.GetComponent<Creature>();
        if (creature == null)
        {
            OnDestroyed?.Invoke(this);
            return;
        }

        if (other.CompareTag(Shooter?.tag)) return;
        creature.Hurt(damage);
        OnDestroyed?.Invoke(this);
    }
}