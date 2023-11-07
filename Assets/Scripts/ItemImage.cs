using UnityEngine.UI;

public class ItemImage : Image
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