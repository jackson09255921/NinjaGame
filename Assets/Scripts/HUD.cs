using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider healthBar;
    public Slider cooldownBar;
    public Image activeWeaponIcon;
    public Image inactiveWeaponIcon;
    public RectTransform requiredItemPanel;
    public RectTransform extraItemPanel;
    public ItemImage requiredItemIconPrefab;
    public ItemImage extraItemIconPrefab;
    readonly List<ItemImage> requiredItemIcons = new();
    public TextMeshProUGUI timeText;

    void Start()
    {
        foreach (ItemManager.Item item in ItemManager.Instance.requiredItems)
        {
            ItemImage image = Instantiate(requiredItemIconPrefab, requiredItemPanel);
            image.Item = item;
            image.color = Color.gray;
            requiredItemIcons.Add(image);
        }
    }

    void Update()
    {
        timeText.text = GameStateManager.Instance.PlayTimeText;
    }

    internal void UpdateEquipment(Weapon active, Weapon inactive)
    {
        if (active != null)
        {
            activeWeaponIcon.sprite = active.icon;
        }
        if (inactive != null)
        {
            inactiveWeaponIcon.sprite = inactive.icon;
        }
    }

    internal void UpdateHealth(float health)
    {
        healthBar.value = health;
    }

    internal void UpdateItems(int[] items)
    {
        foreach (int itemId in items)
        {
            ItemImage image = requiredItemIcons.Find(im => im.Item.id == itemId);
            if (image != null)
            {
                image.color = Color.white;
                continue;
            }
            ItemManager itemManager = ItemManager.Instance;
            if (itemManager.itemDict.TryGetValue(itemId, out ItemManager.Item item))
            {
                image = Instantiate(extraItemIconPrefab, extraItemPanel);
                image.Item = item;
            }
        }
    }
}
