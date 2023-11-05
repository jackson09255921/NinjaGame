using System;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    public int id;
    internal Action<int, Player> playerEnter;
    internal Action<int, Player> playerStay;
    internal Action<int, Player> playerExit;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(playerEnter != null && other.TryGetComponent(out Player player))
        {
            playerEnter(id, player);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(playerStay != null && other.TryGetComponent(out Player player))
        {
            playerStay(id, player);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(playerExit != null && other.TryGetComponent(out Player player))
        {
            playerExit(id, player);
        }
    }
}
