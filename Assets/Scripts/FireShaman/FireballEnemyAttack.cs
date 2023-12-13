using UnityEngine;

public class FireballEnemyAttack : MonoBehaviour
{
    public float speed = 20;
    public EnemyAttackTrigger explode;
    public int damage;
    Collider2D c2;
    Rigidbody2D rb;
    Animator animator;

    void Awake()
    {
        c2 = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        explode.playerEnter = ApplyDamage;
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = facing*speed;
    }

    void TriggerExplode()
    {
        animator.SetTrigger("Explode");
    }

    internal void StartExplode()
    {
        c2.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        explode.gameObject.SetActive(true);
    }

    internal void EndExplode()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke(nameof(TriggerExplode), 0.2f);
    }

    void ApplyDamage(EnemyAttackTrigger trigger, Player player)
    {
        Debug.Log("hit player");
        player.TakeDamage(damage);
    }
}