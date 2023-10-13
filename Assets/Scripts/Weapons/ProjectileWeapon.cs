using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ProjectileWeapon : Weapon
{
    public GameObject projectilePrefab;
    public Vector2 offset;
    ContactFilter2D contactFilter;
    readonly Collider2D[] overlaps = new Collider2D[16];

    void Awake()
    {
        contactFilter = new();
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
    }

    public override void PerformAttack(Player player)
    {
        Vector2 position = player.transform.TransformPoint(offset);
        if (CanShoot(position))
        {
            Instantiate(projectilePrefab, position, player.transform.rotation);
            player.animator.SetTrigger("Attack");
        }
    }

    bool CanShoot(Vector2 position)
    {
        int count = Physics2D.OverlapCircle(position, 0.5f, contactFilter, overlaps); 
        return overlaps.Take(count).All(c => !c.CompareTag("Ground"));
    }
}