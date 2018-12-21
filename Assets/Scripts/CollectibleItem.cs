using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
        [SerializeField] private string itemName;

        private void OnTriggerEnter(Collider other)
        {
                Managers.Managers.Inventory.AddItem(itemName);
                Destroy(gameObject);
        }
}