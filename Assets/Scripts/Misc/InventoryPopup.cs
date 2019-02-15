using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Визуальное отображеие инвентаря
/// </summary>
public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Image equippedBg;
    [SerializeField] private TextMeshProUGUI[] itemLabels;
    [SerializeField] private TextMeshProUGUI selectedItemLabel;
    [SerializeField] private TextMeshProUGUI placeholderLabel;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button useButton;

    private string selectedItem;

    private void Awake() => Messenger.AddListener(GameEvent.ITEM_ADDED, Refresh);
    private void OnDestroy() => Messenger.RemoveListener(GameEvent.ITEM_ADDED, Refresh);

    public void Refresh()
    {
        var inventory = Managers.Managers.Player.Player.Inventory;
        
        if (inventory.EquippedItem == null) equippedBg.gameObject.SetActive(false);
        var itemList = inventory.GetItemList();
        placeholderLabel.gameObject.SetActive(itemList.Count == 0);

        var len = itemIcons.Length;
        for (var i = 0; i < len; i++)
        {
            if (i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                var item = itemList[i];

                var sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize();

                var count = inventory.GetItemCount(item);
                var message = $"x{count}";
                if (item == inventory.EquippedItem)
                {
                    equippedBg.transform.position = new Vector3(itemIcons[i].transform.position.x,
                        equippedBg.transform.position.y, equippedBg.transform.position.z);
                    equippedBg.gameObject.SetActive(true);
                }

                itemLabels[i].text = message;

                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) => { OnItem(item); });
                var trigger = itemIcons[i].GetComponent<EventTrigger>();

                trigger.triggers.Clear();
                trigger.triggers.Add(entry);
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if (!itemList.Contains(selectedItem))
        {
            selectedItem = null;
        }

        if (selectedItem == null)
        {
            selectedItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            selectedItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if (selectedItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }

            selectedItemLabel.text = selectedItem + ":";
        }
    }

    private void OnItem(string item)
    {
        selectedItem = item;
        Refresh();
    }

    public void OnEquip()
    {
        Managers.Managers.Player.Player.Inventory.EquipItem(selectedItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Managers.Player.Player.Inventory.ConsumeItem(selectedItem);
        Refresh();
    }

    public void OpenClose()
    {
        if (isActiveAndEnabled) Close();
        else Open();
    }

    public void Open()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Messenger<bool>.Broadcast(GameEvent.CAMERA_LOCK, true);
        Refresh();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!Managers.Managers.Settings.Isometric)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Messenger<bool>.Broadcast(GameEvent.CAMERA_LOCK, false);
        gameObject.SetActive(false);
    }
}