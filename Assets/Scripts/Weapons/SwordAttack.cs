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
        if (player.horizontalState != Player.HorizontalState.Dash)
        {
            normalAttack.gameObject.SetActive(true);
        }
        else
        {
            dashAttack.gameObject.SetActive(true);
        }
        Destroy(gameObject, 0.2f);
    }
}