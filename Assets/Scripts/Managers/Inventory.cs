using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class Inventory
    {
        public string EquippedItem { get; private set; }

        private Dictionary<string, List<CollectibleItem>> items;

        public Inventory()
        {
            UpdateData(new Dictionary<string, List<CollectibleItem>>());
        }

        public void UpdateData(Dictionary<string, List<CollectibleItem>> items) => this.items = items;
        public Dictionary<string, List<CollectibleItem>> GetData() => items;

        public void AddItem(CollectibleItem item)
        {
            if (items.ContainsKey(item.Name))
                items[item.Name].Add(item);
            else
            {
                items.Add(item.Name,  new List<CollectibleItem> {item});
            }

            Messenger.Broadcast(GameEvent.ITEM_ADDED, MessengerMode.DONT_REQUIRE_LISTENER);
        }

        public List<string> GetItemList() => new List<string>(items.Keys);
        public int GetItemCount(string name) => items.ContainsKey(name) ? items[name].Count : 0;

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
                var lastIndex = items[name].Count - 1;
                var value = items[name][lastIndex].Value;
                items[name].RemoveAt(lastIndex);
                
                if (items[name].Count == 0) items.Remove(name);
                if (name == "health") Managers.Player.Player.ChangeHealth(value);
            }
            else
            {
                return false;
            }

            return true;
        }

    }
}