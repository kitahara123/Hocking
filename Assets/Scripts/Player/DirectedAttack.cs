using UnityEngine;

public class DirectedAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Creature>().Hurt(damage);
        }
    }
}