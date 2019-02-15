using UnityEngine;

/// <summary>
/// При входе в триггер активирует все связанные девайсы
/// </summary>
public class DeviceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    [SerializeField] private bool requireKey;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (requireKey && Managers.Managers.Player.Player.Inventory.EquippedItem != "key") return;
        foreach (var target in targets)
            target.SendMessage("Activate");
    }
    
    private void OnTriggerExit(Collider other)
    {
        foreach (var target in targets)
            target.SendMessage("Deactivate");
    }
}