using UnityEngine;

/// <summary>
/// Базовый класс для контроля скорости
/// </summary>
public class SpeedControl : MonoBehaviour
{
    [SerializeField] protected float baseSpeed = 6.0f;
    protected static float speedModifier = 1;
    protected float speed;

    protected virtual void Awake()
    {
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
        OnSpeedChanged(Managers.Managers.Settings.GlobalSpeed);
    }

    protected virtual void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);

    protected virtual void OnSpeedChanged(float value)
    {
        speedModifier = value;
        speed = baseSpeed * speedModifier;
    }
}