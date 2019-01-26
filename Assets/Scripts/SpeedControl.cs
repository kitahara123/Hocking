using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    [SerializeField] protected float speedModifier = 1;
    [SerializeField] protected float baseSpeed = 6.0f;
    protected float speed;

    protected void Awake() => Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
    protected void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);

    protected virtual void Start()
    {
        OnSpeedChanged(speedModifier);
    }

    protected virtual void OnSpeedChanged(float value)
    {
        speedModifier = value;
        speed = baseSpeed * speedModifier;
    }
}