using UnityEngine;

public class SpeedControl : MonoBehaviour
{
    [SerializeField] protected float baseSpeed = 6.0f;
    protected static float speedModifier = 1;
    protected float speed;
    private Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        Messenger<float>.AddListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);
        OnSpeedChanged(Managers.Managers.Settings.GlobalSpeed);
    }

    protected virtual void OnDestroy() => Messenger<float>.RemoveListener(GameEvent.SPEED_CHANGED, OnSpeedChanged);

    protected virtual void OnSpeedChanged(float value)
    {
        speedModifier = value;
        speed = baseSpeed * speedModifier;
        if (animator != null)
            animator.speed = speedModifier;
    }
}