using System;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    public int id;
    internal Action<EnemyAttackTrigger, Player> playerEnter;
    internal Action<EnemyAttackTrigger, Player> playerStay;
    internal Action<EnemyAttackTrigger, Player> playerExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(playerEnter != null && other.TryGetComponent(out Player player))
        {
            playerEnter(this, player);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(playerStay != null && other.TryGetComponent(out Player player))
        {
            playerStay(this, player);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(playerExit != null && other.TryGetComponent(out Player player))
        {
            playerExit(this, player);
        }
    }
}
