using UnityEngine;
using UnityEngine.UI;

public class ChestItemImage : Image
{
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
}