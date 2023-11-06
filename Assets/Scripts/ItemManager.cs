using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public Item[] requiredItems;
    public Item[] extraItems;
    internal Dictionary<int, Item> itemDict = new();

    void Awake()
    {
        Instance = this;
        requiredItems = requiredItems.DistinctBy(i => i.id).ToArray();
        extraItems = extraItems.Except(requiredItems).DistinctBy(i => i.id).ToArray();
        foreach (Item item in LinqUtility.Concat<Item>(requiredItems, extraItems))
        {
            itemDict.TryAdd(item.id, item);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    [Serializable]
    public struct Item
    {
        public int id;
        public Sprite icon;
    }
}
