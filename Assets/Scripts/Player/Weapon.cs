using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Sprite icon;
    public RuntimeAnimatorController animationController;
    public Attack attackPrefab;
    public Vector2 offset;
    public float checkRadius;
    public bool checkGround = true;
    public bool checkEnemy = false;
    public float cooldown;
    ContactFilter2D contactFilter;
    readonly Collider2D[] overlaps = new Collider2D[16];

    void Awake()
    {
        contactFilter = new();
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
    }

    public virtual bool StartAttack(Player player)
    {
        if (attackPrefab != null && CanSpawn(player.transform))
        {
            player.animator.SetTrigger("Attack");
            return true;
        }
        return false;
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
        if (checkRadius > 0)
        {
            int count = Physics2D.OverlapCircle(playerTransform.TransformPoint(offset), playerTransform.localScale.x*checkRadius, contactFilter, overlaps); 
            return overlaps.Take(count).All(c => (!checkGround || !c.CompareTag("Ground")) && (!checkEnemy || !c.CompareTag("Enemy")));
        }
        return true;
    }
}