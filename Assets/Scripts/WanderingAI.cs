using UnityEngine;

[RequireComponent(typeof(Creature))]
public class WanderingAI : SpeedControl
{
    [SerializeField] private float obstacleRange = 5.0f;
    private bool alive;
    private Creature creature;

    private void Start()
    {
        creature = GetComponent<Creature>();
        alive = creature.Alive;
        creature.OnDeath += Die;
    }

    protected override void OnDestroy()
    {
        creature = GetComponent<Creature>();
        creature.OnDeath -= Die;
        Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    }

    private void Die(Creature creature) => alive = false;

    private void Update()
    {
        if (!alive) return;
        transform.Translate(0, 0, speed * Time.deltaTime);

        var ray = new Ray(transform.position, transform.forward);

        if (Physics.SphereCast(ray, 0.75f, obstacleRange))
        {
            var angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }
}