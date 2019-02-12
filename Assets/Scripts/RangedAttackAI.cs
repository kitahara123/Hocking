using System.Collections;
using UnityEngine;


public class RangedAttackAI : AttackAI
{
    [SerializeField] private Projectile projectilePrefab;

    private Projectile projectile;

    private void Update()
    {
        if (alive && projectile == null)
        {
            StartCoroutine(Attack());

        }
    }

    private IEnumerator Attack()
    {
        cooldown = true;
        projectile = Instantiate(projectilePrefab);
        projectile.Shooter = creature;
        projectile.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
        projectile.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(attackCD);
        cooldown = false;
    }
}