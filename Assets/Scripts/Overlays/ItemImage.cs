using UnityEngine;
using UnityEngine.UI;

public class ItemImage : Image
{
    bool collected = true;
    ItemManager.Item item;

    internal ItemManager.Item Item
    {
        get => item;
        set
        {
            item = value;
            sprite = item.icon;
        }
    }

    public bool Collected
    {
        get => collected;
        set
        {
            collected = value;
            color = collected ? Color.white : Color.gray;
        }
    }
}