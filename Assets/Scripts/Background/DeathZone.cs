using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform spawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerRespawn playerRespawn))
        {
            playerRespawn.Respawn();
        }
    }
}
