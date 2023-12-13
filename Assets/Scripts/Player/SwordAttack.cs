using UnityEngine;

public class SwordAttack : Attack
{
    public AttackTrigger normalAttack;
    public int normalDamage;
    public AttackTrigger dashAttack;
    public int dashDamage;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        normalAttack.enemyEnter = ApplyDamage;
        dashAttack.enemyEnter = ApplyDamage;
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

    void ApplyDamage(AttackTrigger trigger, Enemy enemy)
    {
        switch (param)
        {
            default:
            case 0:
            {
                enemy.TakeDamage(normalDamage);
                break;
            }
            case 1:
            {
                enemy.TakeDamage(dashDamage);
                break;
            }
        }
    }
}
