using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Weapon weapon;
    public int[] itemIds;
    public int healAmount;
    public Collider2D openRangeCollider;
    public Image hintIconPanel;
    public Image hintImagePrefab;
    public Color openRangeColor;
    internal Item[] items;
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
        items = itemIds.Select(i => ItemManager.Instance.GetItem(i, out Item item) ? item : null).Where(i => i != null).ToArray();
        ItemManager.Instance.totalItemCount += items.Length;
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
            requiredCount += items.Length;
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
            foreach (Item item in items)
            {
                enumerator.MoveNext();
                enumerator.Current.sprite = item.icon;
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
                return;
            }
            if (openRangeCollider.IsTouching(other))
            {
                if (player.chest != this)
                {
                    player.chest = this;
                    hintIconPanel.color = openRangeColor;
                }
            }
            else if (player.chest == this)
            {
                player.chest = null;
                hintIconPanel.color = Color.white;
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
            }
            hintIconPanel.color = Color.white;
            hintIconPanel.gameObject.SetActive(false);
        }
    }
}
