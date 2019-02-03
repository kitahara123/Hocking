using UnityEngine;

public class DirectedAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("HIT OR MISS I GUESS THEY NEVER MIS HUH");
        }
    }
}