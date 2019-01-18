using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Managers.Managers.Mission.ObjectiveReached();
    }
}