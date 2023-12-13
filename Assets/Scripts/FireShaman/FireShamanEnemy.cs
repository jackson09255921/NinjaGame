using System;
using System.Linq;
using UnityEngine;

public class FireShamanEnemy : Enemy
{
    public float moveSpeed = 2;
    public float acceleration = 40;
    public float patrolRange = 3;
    public float idleDuration = 1;
    public float attackRange = 3;
    public float attackCooldown = 1;
    public bool doesSpecialAttack = false;
    public int specialAttackCooldown = 5;
    public Transform charmPoint;
    public float groundCheckRadius;
    public FireballEnemyAttack fireball1EnemyAttackPrefab;
    public FireballEnemyAttack fireball2EnemyAttackPrefab;
    //public FireTornadoEnemyAttack fireTornadoAttackPrefab;

    internal Rigidbody2D rb;
    internal Animator animator;
    internal float homeX;
    Player player;
    float lastAttackTime = 0;
    float attackCount = 0;
    float idleTimer;
    bool movingRight = true;
    bool isIdle = false;
    ContactFilter2D contactFilter;
    readonly Collider2D[] overlaps = new Collider2D[16];

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        contactFilter = new();
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
    }

    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<Player>();
        homeX = transform.position.x;
        idleTimer = patrolRange;
    }

    protected override void Update()
    {
        base.Update();
        UpdateAttack();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }

    void UpdateMovement()
    {
        float currentVelocityX = rb.velocity.x;
        float speed = Math.Abs(rb.velocity.x);
        if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
        {
            if (isIdle)
            {
                animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
                idleTimer -= Time.fixedDeltaTime;
                if (idleTimer <= 0)
                {
                    isIdle = false;
                }
            }
            else
            {
                float accelerationX = 0;
                if (movingRight && transform.position.x-homeX < patrolRange)
                {
                    accelerationX = Math.Min(1, (moveSpeed-speed)*4)*acceleration;
                    transform.rotation = Constants.rightRotation;
                    animator.SetFloat("Speed", currentVelocityX < 0.5f ? 0.5f : speed);
                }
                else if (!movingRight && homeX-transform.position.x < patrolRange)
                {
                    accelerationX = -Math.Min(1, (moveSpeed-speed)*4)*acceleration;
                    transform.rotation = Constants.leftRotation;
                    animator.SetFloat("Speed", currentVelocityX > -0.5f ? 0.5f : speed);
                }
                else
                {
                    animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
                    idleTimer = idleDuration;
                    isIdle = true;
                    movingRight = !movingRight;
                }
                rb.AddForce(new(accelerationX*rb.mass, 0), ForceMode2D.Force);
            }
        }
        else
        {
            animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
            movingRight = player.transform.position.x > transform.position.x;
            transform.rotation = movingRight ? Constants.rightRotation : Constants.leftRotation;
            idleTimer = idleDuration;
            isIdle = true;
        }
    }

    void UpdateAttack()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        {
            if (Time.time >= lastAttackTime+attackCooldown)
            {
                lastAttackTime = Time.time;
                if (doesSpecialAttack && attackCount >= specialAttackCooldown)
                {
                    animator.SetTrigger("Skill");
                    attackCount = 0;
                }
                else if (CanFireballAttack())
                {
                    animator.SetTrigger("Attack");
                    attackCount++;
                }
            }
        }
    }

    bool CanFireballAttack()
    {
        if (groundCheckRadius > 0)
        {
            int count = Physics2D.OverlapCircle(charmPoint.position, transform.localScale.x*groundCheckRadius, contactFilter, overlaps); 
            return overlaps.Take(count).All(c => !c.CompareTag("Ground"));
        }
        return true;
    }

    internal void PerformFireball1Attack()
    {
        FireballEnemyAttack attack = Instantiate(fireball1EnemyAttackPrefab, charmPoint.position, charmPoint.rotation);
    }

    internal void PerformLightningAttack()
    {
        //FireTornadoEnemyAttack attack = Instantiate(fireTornadoAttackPrefab, transform.position, transform.rotation);
    }
}
