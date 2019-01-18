using UnityEngine;

public class BaseDevice : MonoBehaviour
{
        [SerializeField] private bool requireKey;
        private void OnMouseDown()
        {
                if (requireKey && Managers.Managers.Inventory.EquippedItem != "key") return;
                Operate();
        }

        public virtual void Operate()
        {
                
        }
}