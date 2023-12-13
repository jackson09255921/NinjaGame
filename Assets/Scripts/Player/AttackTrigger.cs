using System;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int id;
    internal Action<AttackTrigger, Enemy> enemyEnter;
    internal Action<AttackTrigger, Enemy> enemyStay;
    internal Action<AttackTrigger, Enemy> enemyExit;
    internal Collider2D c2D;

    void Awake()
    {
        c2D = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(enemyEnter != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyEnter(this, enemy);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(enemyStay != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyStay(this, enemy);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(enemyExit != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyExit(this, enemy);
        }
    }
}
