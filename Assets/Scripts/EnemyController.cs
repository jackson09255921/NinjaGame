using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2;
    public float acceleration = 40;
    public float patrolRange = 3;
    public float idleDuration = 1;
    public float attackRange = 3;
    public float specialAttackCooldown = 5;
    public float attackCooldown = 1;
    public Transform lightningPoint;
    public GameObject lightningPrefab;

    internal Rigidbody2D rb;
    internal Animator animator;
    internal float homeX;
    Transform player;
    float lastAttackTime = 0;
    float lastSpecialAttackTime = 0;
    float idleTimer;
    bool movingRight = true;
    bool isIdle = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        homeX = transform.position.x;
        idleTimer = patrolRange;
    }

    void Update()
    {
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
        if (Vector2.Distance(transform.position, player.position) > attackRange)
        {
            if (isIdle)
            {
                animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
                idleTimer -= Time.fixedDeltaTime;
                if (idleTimer <= 0)
                {
                    isIdle = false;
                    movingRight = !movingRight;
                }
            }
            else
            {
                float accelerationX = 0;
                if (movingRight && transform.position.x-homeX < patrolRange)
                {
                    accelerationX = Math.Min(1, (moveSpeed-speed)*4)*acceleration;
                    animator.SetFloat("Speed", currentVelocityX < 0.5f ? 0.5f : speed);
                }
                else if (!movingRight && homeX-transform.position.x < patrolRange)
                {
                    accelerationX = -Math.Min(1, (moveSpeed-speed)*4)*acceleration;
                    animator.SetFloat("Speed", currentVelocityX > -0.5f ? 0.5f : speed);
                }
                else
                {
                    animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
                    idleTimer = idleDuration;
                    isIdle = true;
                }
                rb.AddForce(new(accelerationX*rb.mass, 0), ForceMode2D.Force);
            }
        }
        else
        {
            animator.SetFloat("Speed", speed > 1 ? currentVelocityX*0.5f : 0);
            movingRight = player.position.x > transform.position.x;
            idleTimer = idleDuration;
            isIdle = true;
        }
        transform.rotation = movingRight ? Constants.rightRotation : Constants.leftRotation;
    }

    void UpdateAttack()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
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
                // GameObject attackInstance = Instantiate(attackPrefab, attackPoint.position, attackPoint.rotation);
                // Destroy(attackInstance, 0.5f);
            }
        }
    }

    IEnumerator StopForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
