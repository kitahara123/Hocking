using UnityEngine;

public class BaseDevice : SpeedControl
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