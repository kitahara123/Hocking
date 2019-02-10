using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Creature), typeof(WanderingAI))]
public class AgressiveAI : SpeedControl
{
    [SerializeField] private float meleeRange = 1;
    [SerializeField] private float attackPerSecond = 1.5f;
    [SerializeField] private int damage = 20;
    private bool alive;
    private Creature creature;
    private WanderingAI wanderingAi;
    private bool cooldown;
    private bool aggro;
    private Transform player;

    private void Start()
    {
        creature = GetComponent<Creature>();
        wanderingAi = GetComponent<WanderingAI>();
        alive = creature.Alive;
        creature.OnDeath += Die;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        creature = GetComponent<Creature>();
        creature.OnDeath -= Die;
        
    }

    private void Update()
    {
        if (!alive) return;
        if (!aggro) return;
        
        var distance = Vector3.Distance(player.position, transform.position);
        if (meleeRange < distance)
        {
            var direction = player.position - transform.position;
            var rot = Quaternion.LookRotation(direction, transform.forward);
            transform.rotation = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        if (!cooldown && meleeRange >= distance)
            StartCoroutine(Attack());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alive || !other.CompareTag("Player")) return;

        wanderingAi.enabled = false;
        aggro = true;
        player = other.transform;

    }

    private void OnTriggerExit(Collider other)
    {
        if (!alive || !other.CompareTag("Player")) return;
        wanderingAi.enabled = true;
        aggro = false;
    }

    private IEnumerator Attack()
    {
        cooldown = true;
        player.gameObject.GetComponent<PlayerCharacter>().Hurt(damage);
        yield return new WaitForSeconds(attackPerSecond);
        cooldown = false;
    }

    private void Die(Creature creature) => alive = false;
}