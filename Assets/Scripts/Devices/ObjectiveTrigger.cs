using UnityEngine;

/// <summary>
/// Триггер завершения уровня 
/// </summary>
public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        Managers.Managers.Mission.ObjectiveReached();
    }
}