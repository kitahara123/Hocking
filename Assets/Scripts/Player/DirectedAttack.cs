using UnityEngine;

/// <summary>
/// Направленная атака
/// </summary>
public class DirectedAttack : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string enemyTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            other.GetComponent<Creature>().Hurt(damage);
        }
    }
}