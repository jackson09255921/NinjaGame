using UnityEngine;

public class FireTornadoEnemyAttack : MonoBehaviour
{
    public float speed = 0.1f;
    public int damage;
    public float damageInterval = 0.2f;
    public float duration = 1f;
    Collider2D c2;
    Rigidbody2D rb;
    float lastDamageTime = float.NegativeInfinity;

    void Awake()
    {
        c2 = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = facing*speed;
    }

    internal void StartTornado()
    {
        c2.enabled = true;
    }

    internal void EndTornado()
    {
        Destroy(gameObject);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(Time.time-lastDamageTime >= damageInterval && other.TryGetComponent(out Player player))
        {
            player.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}