using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Weapon weapon;
    public int[] itemIds;
    public GameObject hintCanvasElement;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Empty)
        {
            return;
        }
        if (other.TryGetComponent(out Player player))
        {
            player.chest = this;
            hintCanvasElement.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (player.chest != this || Empty)
            {
                hintCanvasElement.SetActive(false);
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
                hintCanvasElement.SetActive(false);
            }
        }
    }
}
