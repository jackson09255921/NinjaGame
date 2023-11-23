using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShurikenAttack : Attack
{
    public int damage;
    public List<AttackProjectile> projectiles;
    public float speed = 20;
    public float gapAngle = 15;
    public float angularSpeed = 360;

    void Start()
    {
        int count = projectiles.Count;
        float startAngle = gapAngle*(count-1)/2;
        Vector2 facing = transform.right;
        for (int i = 0; i < count; ++i)
        {
            AttackProjectile p = projectiles[i];
            p.enemyEnter = ApplyDamage;
            foreach (AttackProjectile p1 in projectiles)
            {
                Physics2D.IgnoreCollision(p.c2D, p1.c2D);
            }
            float angle = startAngle-i*gapAngle;
            p.rb.velocity = new Vector2(player.rb.velocity.x, 0)+AngleVector(angle)*speed;
            p.rb.angularVelocity = facing.x*angularSpeed;
        }
        Destroy(gameObject, 0.5f);
    }

    void ApplyDamage(AttackProjectile projectile, Enemy enemy)
    {
        enemy.TakeDamage(damage);
        Destroy(projectile);
    }

    Vector2 AngleVector(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return transform.TransformVector(new(Mathf.Cos(angle), Mathf.Sin(angle)));
    }
}
