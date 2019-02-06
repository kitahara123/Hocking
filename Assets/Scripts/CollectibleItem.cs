using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int value;
    public int Value => value;
    public string Name => itemName;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Managers.Managers.Player.Player.Inventory.AddItem(this);
        Destroy(gameObject);
    }
}