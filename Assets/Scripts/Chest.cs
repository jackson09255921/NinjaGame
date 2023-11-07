using UnityEngine;
using UnityEngine.Rendering;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public Weapon weapon;
    public int[] itemIds;
    bool open;

    public bool Open {
        get => open;
        internal set
        {
            open = value;
            animator.SetBool("open", open);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.chest = this;
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
        }
    }
}
