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
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI gameTimeText;

    void Start()
    {
        foreach (ItemManager.Item item in ItemManager.Instance.requiredItems)
        {
            ItemImage image = Instantiate(requiredItemIconPrefab, requiredItemPanel);
            image.Item = item;
            image.Collected = false;
            requiredItemIcons.Add(image);
        }
    }

    void Update()
    {
        totalTimeText.text = GameStateManager.Instance.TotalTimeText;
        gameTimeText.text = GameStateManager.Instance.GameTimeText;
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

    internal void UpdateCooldown(float cooldown, float maxCooldown)
    {
        cooldownBar.value = cooldown > 0 && maxCooldown > 0 ? cooldown/maxCooldown : 0;
    }

    internal void UpdateItems(int[] items)
    {
        foreach (int itemId in items)
        {
            ItemImage image = requiredItemIcons.Find(im => im.Item.id == itemId);
            if (image != null && !image.Collected)
            {
                image.Collected = true;
                continue;
            }
            if (ItemManager.Instance.itemDict.TryGetValue(itemId, out ItemManager.Item item))
            {
                image = Instantiate(extraItemIconPrefab, extraItemPanel);
                image.Item = item;
            }
        }
    }
}
