using System.Collections;
using UnityEngine;


public class RangedAttackAI : AttackAI
{
    [SerializeField] private Fireball fireballPrefab;

    private Fireball fireball;

    private void Update()
    {
        if (alive && fireball == null)
        {
            StartCoroutine(Attack());

        }
    }

    private IEnumerator Attack()
    {
        cooldown = true;
        fireball = Instantiate(fireballPrefab);
        fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
        fireball.transform.rotation = transform.rotation;
        yield return new WaitForSeconds(attackCD);
        cooldown = false;
    }
}