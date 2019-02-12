using UnityEngine;

[RequireComponent(typeof(Creature))]
public class AttackAI : SpeedControl
{
    [SerializeField] private float calibrateSpeedDependence = 1.9f;
    [SerializeField] protected float baseAttackCD = 1f;
    protected bool cooldown;
    protected float attackCD;

    protected Creature creature;

    protected override void Awake()
    {
        base.Awake();
        creature = GetComponent<Creature>();
    }

    private void OnEnable()
    {
        cooldown = false;
    }

    protected override void OnSpeedChanged(float value)
    {
        base.OnSpeedChanged(value);
        if (speedModifier != 0)
            attackCD = baseAttackCD * (calibrateSpeedDependence - Mathf.Log(speedModifier * speedModifier));
    }
}