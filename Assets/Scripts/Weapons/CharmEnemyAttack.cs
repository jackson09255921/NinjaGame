using UnityEngine;

public class CharmEnemyAttack : MonoBehaviour
{
    public bool large;
    public float speed = 20;
    public EnemyAttackTrigger smallExplode;
    public int smallDamage;
    public EnemyAttackTrigger largeExplode;
    public int largeDamage;
    Collider2D c2;
    Rigidbody2D rb;
    Animator animator;

    void Awake()
    {
        c2 = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        smallExplode.playerEnter = ApplyDamage;
        largeExplode.playerEnter = ApplyDamage;
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = facing*speed;
        animator.SetBool("Large", large);
    }

    internal void StartExplode()
    {
        c2.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        if (!large)
        {
            smallExplode.gameObject.SetActive(true);
        }
        else
        {
            largeExplode.gameObject.SetActive(true);
        }
    }

    internal void EndExplode()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Explode");
    }

    void ApplyDamage(int id, Player player)
    {
        Debug.Log("hit player");
        if (!large)
        {

        }
        else
        {

        }
    }
}