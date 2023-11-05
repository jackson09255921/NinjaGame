using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Enemy
{
    public float moveSpeed = 2;
    public float acceleration = 40;
    public float patrolRange = 3;
    public float idleDuration = 1;
    public float attackRange = 3;
    public float specialAttackCooldown = 5;
    public float attackCooldown = 1;
    public Transform charmPoint;
    public CharmEnemyAttack charmAttackPrefab;
    public Transform lightningPoint;
    public GameObject lightningPrefab;

    internal Rigidbody2D rb;
    internal Animator animator;
    internal float homeX;
    Player player;
    float lastAttackTime = 0;
    float lastSpecialAttackTime = 0;
    float idleTimer;
    bool movingRight = true;
    bool isIdle = false;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
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
            if (Time.time >= lastSpecialAttackTime+specialAttackCooldown)
            {
                lastAttackTime = Time.time;
                lastSpecialAttackTime = Time.time;
                animator.SetTrigger("Skill");
                // GameObject lightningInstance = Instantiate(lightningPrefab, lightningPoint.position, lightningPoint.rotation);
                // Destroy(lightningInstance, 0.5f);
            }
            else if (Time.time >= lastAttackTime+attackCooldown)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("Attack");
            }
        }
    }

    internal void PerformAttack()
    {
        CharmEnemyAttack attack = Instantiate(charmAttackPrefab, charmPoint.position, charmPoint.rotation);
    }
}
