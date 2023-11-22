using UnityEngine;

public class ShurikenAttack : Attack
{
    public int damage;
    public float speed = 20;
    public float angularSpeed = 360;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = new Vector2(player.rb.velocity.x, 0)+facing*speed;
        rb.angularVelocity = facing.x*angularSpeed;
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
