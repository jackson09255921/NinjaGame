using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public Animator animator;
    bool open;

    public bool Open {
        get => open;
        internal set {
            open = value;
            animator.SetBool("open", open);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!open)
        {
            if (other.TryGetComponent(out Player player))
            {
                player.treasure = this;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.treasure == this)
            {
                player.treasure = null;
                player.chestMenu.gameObject.SetActive(false);
            }
        }
    }
}
