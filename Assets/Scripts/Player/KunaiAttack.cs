using UnityEngine;

public class KunaiAttack : Attack
{
    public int damage;
    public float speed = 20;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = new Vector2(player.rb.velocity.x, 0)+facing*speed;
        Destroy(gameObject, 0.5f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
