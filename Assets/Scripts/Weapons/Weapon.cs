using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Sprite icon;
    public AnimatorController animationController;
    public Attack attackPrefab;
    public Vector2 offset;
    public float groundCheckRadius;
    ContactFilter2D contactFilter;
    readonly Collider2D[] overlaps = new Collider2D[16];

    void Awake()
    {
        contactFilter = new();
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
    }

    public virtual void PerformAttack(Player player)
    {
        Vector2 position = player.transform.TransformPoint(offset);
        if (attackPrefab != null && CanSpawn(position))
        {
            Attack attack = Instantiate(attackPrefab, position, player.transform.rotation);
            attack.player = player;
            player.animator.SetTrigger("Attack");
        }
    }

    bool CanSpawn(Vector2 position)
    {
        if (groundCheckRadius > 0)
        {
            int count = Physics2D.OverlapCircle(position, groundCheckRadius, contactFilter, overlaps); 
            return overlaps.Take(count).All(c => !c.CompareTag("Ground"));
        }
        return true;
    }
}