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
    public RectTransform extraItemPanel2;
    public ItemImage requiredItemIconPrefab;
    public ItemImage extraItemIconPrefab;
    readonly List<ItemImage> requiredItemIcons = new();
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI gameTimeText;

    void Start()
    {
        foreach (Item item in ItemManager.Instance.requiredItems)
        {
            ItemImage image = Instantiate(requiredItemIconPrefab, requiredItemPanel);
            image.Item = item;
            image.Collected = false;
            requiredItemIcons.Add(image);
        }
        extraItemPanel2.sizeDelta *= new Vector2(ItemManager.Instance.extraRequirement, 1);
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

    internal void UpdateItems(Item[] items)
    {
        foreach (Item item in items)
        {
            ItemImage image = requiredItemIcons.Find(im => im.Item == item);
            if (image != null && !image.Collected)
            {
                image.Collected = true;
                continue;
            }
            image = Instantiate(extraItemIconPrefab, extraItemPanel.transform);
            image.Item = item;
        }
    }
}
