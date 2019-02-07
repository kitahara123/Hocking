using UnityEngine;

public class DirectedAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [Header("Чем больше значение тем дольше будет существовать")]
    [SerializeField] private float destroyCalibrate = 2.4f;

    private void Awake()
    {
        Destroy(gameObject,
            destroyCalibrate -
            Mathf.Log(Managers.Managers.Settings.GlobalSpeed * Managers.Managers.Settings.GlobalSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Creature>().Hurt(damage);
        }
    }
}