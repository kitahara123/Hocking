using UnityEngine;

[RequireComponent(typeof(Creature))]
public class AttackAI : MonoBehaviour
{
    [SerializeField]protected int attackCD = 1;
    protected bool cooldown;
    protected bool alive;

    private Creature creature;
    protected virtual void Awake()
    {
        creature = GetComponent<Creature>();
        alive = creature.Alive;
        creature.OnDeath += Die;

    }

    protected virtual void OnDestroy()
    {
        creature = GetComponent<Creature>();
        creature.OnDeath -= Die;
    }

    protected virtual void Die(Creature creature) => alive = false;
}