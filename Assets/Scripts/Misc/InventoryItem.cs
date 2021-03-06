using System;

/// <summary>
/// Сериализуемый класс для сохранения данных инвентаря
/// </summary>
[Serializable]
public class InventoryItem
{
    private int value;
    private string name;
    public int Value => value;
    public string Name => name;

    public InventoryItem(int value, string name)
    {
        this.value = value;
        this.name = name;
    }
}