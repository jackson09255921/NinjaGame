using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRespawn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn playerRespawn = other.GetComponent<PlayerRespawn>();
            if (playerRespawn != null)
            {
                playerRespawn.SetSpawnPoint(transform);
                Debug.Log("Change SpawnPoint");
            }
        }
    }
}
