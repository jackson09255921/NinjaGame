using UnityEngine;

public class SwordAttack : Attack
{
    public Collider2D normalAttack;
    public Collider2D dashAttack;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = player.rb.velocity;
        switch (param)
        {
            default:
            case 0:
            {
                normalAttack.gameObject.SetActive(true);
                break;
            }
            case 1:
            {
                dashAttack.gameObject.SetActive(true);
                break;
            }
        }
        Destroy(gameObject, 0.2f);
    }
}