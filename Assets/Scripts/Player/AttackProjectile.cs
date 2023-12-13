using System;
using UnityEngine;

public class AttackProjectile : MonoBehaviour
{
    public int id;
    internal Action<AttackProjectile, Enemy> enemyEnter;
    internal Action<AttackProjectile, Enemy> enemyStay;
    internal Action<AttackProjectile, Enemy> enemyExit;
    internal Rigidbody2D rb;
    internal Collider2D c2D;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        c2D = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(enemyEnter != null && collision.collider.TryGetComponent(out Enemy enemy))
        {
            enemyEnter(this, enemy);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(enemyStay != null && collision.collider.TryGetComponent(out Enemy enemy))
        {
            enemyStay(this, enemy);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(enemyExit != null && collision.collider.TryGetComponent(out Enemy enemy))
        {
            enemyExit(this, enemy);
        }
    }
}
