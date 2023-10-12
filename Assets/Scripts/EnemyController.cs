using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float patrolDuration = 3f; // Time to patrol in one direction (seconds)
    public float attackRange = 3f;
    public float specialAttackCooldown = 5f;
    public float attackCooldown = 1f;

    private Transform player;
    private Animator animator;
    private bool movingRight = true;
    private float nextAttackTime;
    private float specialAttackTime;
    private float patrolTimer; // Timer for patrolling in one direction
    private bool isIdle = false;
    // Attack
    public Transform lightningPoint;
    public GameObject lightningPrefab;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        nextAttackTime = Time.time;
        specialAttackTime = Time.time + specialAttackCooldown;
        patrolTimer = patrolDuration; // Initialize patrol timer
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // Check if player is in attack range
        if (distanceToPlayer <= attackRange)
        {
            // Attack the player
            AttackPlayer();
        }
        else
        {
            // Patrol within patrol duration
            Patrol();
        }
        Flip();
    }

    void Patrol()
    {
        if (isIdle)
        {
            // Reduce the idle timer
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0)
            {
                isIdle = false; // End the idle state
                patrolTimer = patrolDuration;
                movingRight = !movingRight;
            }
        }
        else
        {
            animator.SetTrigger("Move");
            // Reduce the patrol timer
            patrolTimer -= Time.deltaTime;
            if (patrolTimer > 0)
            {
                transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);
            }
            else
            {
                animator.SetTrigger("Idle");
                isIdle = true;
            }
        }
    }

    void AttackPlayer()
    {
        // Face the player
        movingRight = transform.position.x < player.position.x;

        // Attack the player
        // Check if it's time for a special attack
        if (Time.time >= specialAttackTime)
        {
            // Perform special attack
            animator.SetTrigger("Skill");
            // GameObject lightningInstance = Instantiate(lightningPrefab, lightningPoint.position, lightningPoint.rotation);
            // Destroy(lightningInstance, 0.5f);
            specialAttackTime = Time.time + specialAttackCooldown; // Reset special attack cooldown
        }
        else if (Time.time >= nextAttackTime)
        {
            // Perform a regular attack
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + attackCooldown; // Reset attack cooldown
        }
    }

    IEnumerator StopForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void Flip()
    {
        Quaternion rotation = transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        eulerAngles.y = movingRight ? 0 : 180;
        rotation.eulerAngles = eulerAngles;
        transform.rotation = rotation;
    }
}
