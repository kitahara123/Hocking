using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Creature), typeof(WanderingAI))]
public class AggressiveRangedAI : SpeedControl
{
    [SerializeField] private int strafeRange = 3;
    private Creature creature;
    private WanderingAI wanderingAi;
    private Vector3 startPosition;

    private void Start()
    {
        creature = GetComponent<Creature>();
        wanderingAi = GetComponent<WanderingAI>();
    }

    // При попадании снаряда игрока, разворачивается в его сторону и начинает стрейфиться из стороны в сторону
    private void OnTriggerEnter(Collider other)
    {
        if (!creature.Alive) return;
        var proj = other.GetComponent<Projectile>();
        if (proj == null) return;
        if (!proj.Shooter.CompareTag("Player")) return;
        StartCoroutine(Strafe(proj.Shooter));
    }

    private IEnumerator Strafe(Creature shooter)
    {
        wanderingAi.enabled = false;
        startPosition = transform.position;

        while (shooter.Alive && creature.Alive)
        {
            transform.LookAt(shooter.transform);
            if (Vector3.Distance(transform.position, startPosition) > strafeRange)
            {
                speed = -speed;
            }

            transform.Translate(speed * Time.deltaTime, 0, 0);
            yield return null;
        }
        
        wanderingAi.enabled = true;
    }
}