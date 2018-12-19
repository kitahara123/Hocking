using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
        [SerializeField] private string itemName;

        private void OnTriggerEnter(Collider other)
        {
                Debug.Log("item collected: "  + itemName);
                Destroy(gameObject);
        }
}