using UnityEngine;
using UnityEngine.UI;

public class ItemImage : Image
{
    bool collected = true;
    Item item;

    internal Item Item
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