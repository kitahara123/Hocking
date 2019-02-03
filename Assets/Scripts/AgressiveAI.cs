using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Creature), typeof(WanderingAI))]
public class AgressiveAI : SpeedControl
{
    [SerializeField] private int aggressionRange = 10;
    [SerializeField] private float meleeRange = 1;
    [SerializeField] private float attackPerSecond = 1.5f;
    [SerializeField] private int damage = 20;
    private bool alive;
    private Creature creature;
    private WanderingAI wanderingAi;
    private PlayerCharacter player;
    private bool cooldown;

    private void Start()
    {
        creature = GetComponent<Creature>();
        wanderingAi = GetComponent<WanderingAI>();
        player = FindObjectOfType<PlayerCharacter>();
        alive = creature.Alive;
        creature.OnDeath += Die;
    }

    protected override void OnDestroy()
    {
        creature = GetComponent<Creature>();
        creature.OnDeath -= Die;
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }


    private void Update()
    {
        if (!alive) return;

        var distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < aggressionRange)
        {
            wanderingAi.enabled = false;
            transform.LookAt(player.transform.position);
            if (meleeRange < distance)
                transform.Translate(0, 0, speed * Time.deltaTime);

            if (!cooldown && meleeRange >= distance)
                StartCoroutine(Attack());
        }
        else
        {
            wanderingAi.enabled = true;
        }
    }

    private IEnumerator Attack()
    {
        cooldown = true;
        player.Hurt(damage);
        yield return new WaitForSeconds(attackPerSecond);
        cooldown = false;
    }

    private void Die(Creature creature) => alive = false;
}