using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public int damage;
    public Transform respawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            player.transform.position = respawnPoint.position;
        }
    }
}
