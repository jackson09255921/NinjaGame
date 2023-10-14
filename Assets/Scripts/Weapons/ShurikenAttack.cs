using UnityEngine;

public class ShurikenAttack : Attack
{
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
        rb.velocity = facing*speed;
        rb.angularVelocity = facing.x*angularSpeed;
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
