using System.Collections.Generic;
using UnityEngine;

public class BasicUI : MonoBehaviour
{
    [SerializeField] private int startPosX = 100;
    [SerializeField] private int startPosY = 10;
    [SerializeField] private int width = 100;
    [SerializeField] private int height = 30;
    [SerializeField] private int buffer = 10;

    private void OnGUI()
    {
        int posX = startPosX;
        int posY = startPosY;

        List<string> itemList = Managers.Managers.Inventory.GetItemList();

        if (itemList.Count == 0)
        {
            GUI.Box(new Rect(posX, posY, width, height), "No Items");
        }

        foreach (var item in itemList)
        {
            var count = Managers.Managers.Inventory.GetItemCount(item);
            var image = Resources.Load<Texture2D>("Icons/" + item);
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent($"({count})", image));

            if (Managers.Managers.Inventory.Opened)
            {
                posY += height + buffer;

                if (item == "health")
                {
                    if (GUI.Button(new Rect(posX, posY, width, height), "Use " + item)
                    ) // да я понимаю какое это говно, но сейчас я делаю по книжке
                    {
                        Managers.Managers.Inventory.ConsumeItem(item);
                        Managers.Managers.Player.AddHealth(25);
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(posX, posY, width, height), "Equip " + item))
                        Managers.Managers.Inventory.EquipItem(item);
                }
            }

            posY = startPosY;
            posX += width + buffer;
        }

        var equipped = Managers.Managers.Inventory.EquippedItem;
        if (equipped != null)
        {
            posX = Screen.width - (width + buffer);
            var image = Resources.Load("Icons/" + equipped) as Texture2D;
            GUI.Box(new Rect(posX, posY, width, height), new GUIContent("Equipped", image));
        }
    }
}