using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerRespawn playerRespawn))
        {
            playerRespawn.SetSpawnPoint(transform);
            Debug.Log("Change SpawnPoint");
        }
    }
}
