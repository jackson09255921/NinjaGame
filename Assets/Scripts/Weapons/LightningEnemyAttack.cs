using UnityEngine;

public class LightningEnemyAttack : MonoBehaviour
{
    public int damage;
    Collider2D c2;

    void Awake()
    {
        c2 = GetComponent<Collider2D>();
    }

    internal void StartLightning()
    {
        c2.enabled = true;
    }

    internal void EndLightning()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Player player))
        {
            Debug.Log("hit player");
            player.TakeDamage(damage);
        }
    }
}