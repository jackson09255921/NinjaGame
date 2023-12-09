using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Weapon weapon;
    public int[] itemIds;
    public Collider2D openRangeCollider;
    public Image hintIconPanel;
    public Image hintImagePrefab;
    public Color openRangeColor;
    public RectTransform hintButtonPanel;
    readonly LinkedList<Image> itemIcons = new();
    bool open;

    public bool Open
    {
        get => open;
        internal set
        {
            open = value;
            animator.SetBool("open", open);
        }
    }

    public bool Empty
    {
        get => open && weapon == null;
    }

    void Start()
    {
        UpdateHint();
    }

    internal void UpdateHint()
    {
        int requiredCount = 0;
        if (weapon != null)
        {
            requiredCount += 1;
        }
        if (!open)
        {
            requiredCount += itemIds.Length;
        }
        while (itemIcons.Count < requiredCount)
        {
            itemIcons.AddLast(Instantiate(hintImagePrefab, hintIconPanel.transform));
        }
        while (itemIcons.Count > requiredCount)
        {
            Destroy(itemIcons.Last.Value.gameObject);
            itemIcons.RemoveLast();
        }
        var enumerator = itemIcons.GetEnumerator();
        if (weapon != null)
        {
            enumerator.MoveNext();
            enumerator.Current.sprite = weapon.icon;
        }
        if (!open)
        {
            foreach (int itemId in itemIds)
            {
                enumerator.MoveNext();
                if (ItemManager.Instance.itemDict.TryGetValue(itemId, out ItemManager.Item item))
                {
                    enumerator.Current.sprite = item.icon;
                }
            }
        }
        enumerator.Dispose();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Empty)
        {
            return;
        }
        if (other.TryGetComponent(out Player _))
        {
            hintIconPanel.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (Empty)
            {
                hintIconPanel.color = Color.white;
                hintIconPanel.gameObject.SetActive(false);
                hintButtonPanel.gameObject.SetActive(false);
                return;
            }
            if (openRangeCollider.IsTouching(other))
            {
                if (player.chest != this)
                {
                    player.chest = this;
                    hintIconPanel.color = openRangeColor;
                    hintButtonPanel.gameObject.SetActive(true);
                }
            }
            else if (player.chest == this)
            {
                player.chest = null;
                hintIconPanel.color = Color.white;
                hintButtonPanel.gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.chest == this)
            {
                player.chest = null;
                hintButtonPanel.gameObject.SetActive(false);
            }
            hintIconPanel.color = Color.white;
            hintIconPanel.gameObject.SetActive(false);
        }
    }
}
