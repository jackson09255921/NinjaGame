using System;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public int extraRequirement = 1;
    public Item[] items;
    internal Item[] requiredItems;
    internal Item[] extraItems;
    internal int totalItemCount = 0;

    void Awake()
    {
        Instance = this;
        requiredItems = items.Where(i => i.required).ToArray();
        extraItems = items.Where(i => !i.required).ToArray();
        for(int i = 0; i < items.Length; ++i)
        {
            items[i].id = i;
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public bool GetItem(int id, out Item item)
    {
        if (id >= 0 && id < items.Length)
        {
            item = items[id];
            return true;
        }
        item = null;
        return false;
    }
}
