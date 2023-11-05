using System;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public int id;
    internal Action<int, Enemy> enemyEnter;
    internal Action<int, Enemy> enemyStay;
    internal Action<int, Enemy> enemyExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(enemyEnter != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyEnter(id, enemy);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(enemyStay != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyStay(id, enemy);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(enemyExit != null && other.TryGetComponent(out Enemy enemy))
        {
            enemyExit(id, enemy);
        }
    }
}
