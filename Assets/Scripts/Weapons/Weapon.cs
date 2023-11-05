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

    public virtual void StartAttack(Player player)
    {
        if (attackPrefab != null && CanSpawn(player.transform))
        {
            player.animator.SetTrigger("Attack");
        }
    }

    public virtual void PerformAttack(Player player, int param)
    {
        Vector2 position = player.transform.TransformPoint(offset);
        Attack attack = Instantiate(attackPrefab, position, player.transform.rotation);
        attack.player = player;
        attack.param = param;
    }

    bool CanSpawn(Transform playerTransform)
    {
        if (groundCheckRadius > 0)
        {
            int count = Physics2D.OverlapCircle(playerTransform.TransformPoint(offset), playerTransform.localScale.x*groundCheckRadius, contactFilter, overlaps); 
            return overlaps.Take(count).All(c => !c.CompareTag("Ground"));
        }
        return true;
    }
}