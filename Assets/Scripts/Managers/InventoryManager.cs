using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private int healthKitDelta = 25;

        public ManagerStatus status { get; private set; }
        public string EquippedItem { get; private set; }

        public bool Opened { get; private set; }
        public bool IsEmpty => items.Count == 0;
        private Dictionary<string, int> items;

        public void Startup(NetworkService service)
        {
            Debug.Log("Inventory manager starting...");

            Opened = false;
            UpdateData(new Dictionary<string, int>());
            status = ManagerStatus.Started;
        }

        public void UpdateData(Dictionary<string, int> items) => this.items = items;
        public Dictionary<string, int> GetData() => items;

        public void AddItem(string name)
        {
            if (items.ContainsKey(name))
                items[name] += 1;
            else
                items[name] = 1;
            Messenger.Broadcast(GameEvent.ITEM_ADDED, MessengerMode.DONT_REQUIRE_LISTENER);
        }

        public List<string> GetItemList() => new List<string>(items.Keys);
        public int GetItemCount(string name) => items.ContainsKey(name) ? items[name] : 0;

        public bool EquipItem(string name)
        {
            if (items.ContainsKey(name) && EquippedItem != name)
            {
                EquippedItem = name;
                return true;
            }

            EquippedItem = null;
            return false;
        }

        public bool ConsumeItem(string name)
        {
            if (items.ContainsKey(name))
            {
                items[name]--;
                if (items[name] == 0) items.Remove(name);
                if (name == "health") Managers.Player.ChangeHealth(healthKitDelta);
            }
            else
            {
                return false;
            }

            return true;
        }

        public void Open()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Messenger<bool>.Broadcast(GameEvent.CAMERA_LOCK, true);
            Opened = true;
        }

        public void Close()
        {
            if (!Managers.Settings.Isometric)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            Messenger<bool>.Broadcast(GameEvent.CAMERA_LOCK, false);
            Opened = false;
        }

        public void OpenClose()
        {
            if (Opened) Close();
            else Open();
        }
    }
}