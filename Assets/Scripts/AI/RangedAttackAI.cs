using System.Collections;
using UnityEngine;

/// <summary>
/// Просто стреляет снарядами по кд
/// </summary>
public class RangedAttackAI : AttackAI
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float offset = 1.5f;

    private MonoObjectsPool<Projectile> projPool;
    private Projectile projectile;

    private void Start()
    {
        projPool = new MonoObjectsPool<Projectile>(projectilePrefab);
    }

    private void Update()
    {
        if (creature.Alive && !cooldown && speedModifier > 0) StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        cooldown = true;
        projectile = projPool.CreateInstance(10);
        projectile.OnDestroyed += OnProjectileDestroyed;
        projectile.Shooter = creature;
        projectile.transform.position = transform.TransformPoint(Vector3.forward * offset);
        projectile.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(attackCD);
        cooldown = false;
    }

    private void OnProjectileDestroyed(Projectile obj) => projPool.RemoveInstance(obj);
}