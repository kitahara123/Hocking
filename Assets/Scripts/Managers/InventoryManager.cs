using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour, IGameManager
    {
        public ManagerStatus status { get; private set; }
        public string EquippedItem { get; private set; }

        public bool Opened { get; private set; }
        private Dictionary<string, int> items;

        public void Startup(NetworkService service)
        {
            Debug.Log("Inventory manager starting...");

            Opened = false;
            items = new Dictionary<string, int>();
            status = ManagerStatus.Started;
        }

        private void DisplayItems()
        {
            string itemDisplay = "Items: ";
            foreach (KeyValuePair<string, int> item in items)
            {
                itemDisplay += $"{item.Key}({item.Value}) ";
            }
            Debug.Log(itemDisplay);
        }

        public void AddItem(string name)
        {
            if (items.ContainsKey(name))
                items[name] += 1;
            else
                items[name] = 1;
            
            DisplayItems();
        }

        public List<string> GetItemList() => new List<string>(items.Keys);
        public int GetItemCount(string name) => items.ContainsKey(name) ? items[name] : 0;

        public bool EquipItem(string name)
        {
            if (items.ContainsKey(name) && EquippedItem != name)
            {
                EquippedItem = name;
                Debug.Log($"Equipped {name}");
                return true;
            }

            EquippedItem = null;
            Debug.Log("Unequipped");
            return false;
        }

        public bool ConsumeItem(string name)
        {
            if (items.ContainsKey(name))
            {
                items[name]--;
                if (items[name] == 0) items.Remove(name);
            }
            else
            {
                Debug.Log($"Cannot consume {name}");
                return false;
            }
            DisplayItems();
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