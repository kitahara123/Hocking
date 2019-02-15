using UnityEngine;

/// <summary>
/// Базовый класс для всех девайсов
/// </summary>
public class BaseDevice : SpeedControl
{
        [SerializeField] private bool requireKey;
        private void OnMouseDown()
        {
                if (requireKey && Managers.Managers.Player.Player.Inventory.EquippedItem != "key") return;
                Operate();
        }

        public virtual void Operate()
        {
                
        }
}