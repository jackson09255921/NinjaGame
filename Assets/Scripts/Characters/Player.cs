using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Serialized fields
    public float runSpeed = 5;
    public float aerialSpeed = 2;
    public float dashSpeed = 10;
    public float acceleration = 40;
    public float deceleration = 20;
    public float jumpSpeed = 15;
    public int jumpBufferFrames = 5;
    public float jumpSlowFactor = 0.5f;
    public float dashRunTime = 2;
    public float minGroundNormalY = 0.65f;
    public Weapon activeWeapon;
    public Weapon inactiveWeapon;
    public int maxHealth;
    public float flashTime;
    public Color flashColor = Color.red;

    // Components
    internal Rigidbody2D rb;
    internal Animator animator;
    SpriteRenderer sr;

    // Inputs
    InputManager inputManager;
    InputAction jumpAction;
    InputAction horizontalAction;
    InputAction equipmentAction;
    InputAction attackAction;
    InputAction interactAction;

    // Movement fields
    internal Vector3 startPosition;
    internal Quaternion startRotation;
    internal HorizontalState horizontalState = HorizontalState.Idle;
    Vector2 lastVelocity;
    float targetVelocityX = 0;
    int direction = 0;
    float runTime = 0;
    int jumpInput = 0;
    bool stoppedJump = false;
    int jumpTicks = 0;
    readonly ContactPoint2D[] contacts = new ContactPoint2D[16];
    Vector2 maxYNormal;
    bool grounded;

    // Attack fields
    float attackCooldown;
    float attackCooldownMax;

    // Health fields
    internal int health;
    Color originalColor;

    // Interactable fields
    internal Chest chest;
    internal List<ItemManager.Item> collectedItems = new();
    HUD hud;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        jumpAction = inputManager.FindAction("Default/Jump");
        horizontalAction = inputManager.FindAction("Default/Horizontal");
        equipmentAction = inputManager.FindAction("Default/Equipment");
        attackAction = inputManager.FindAction("Default/Attack");
        interactAction = inputManager.FindAction("Default/Interact");
        hud = GameStateManager.Instance.hud;
        hud.UpdateEquipment(activeWeapon, inactiveWeapon);
        health = maxHealth;
        hud.UpdateHealth(1);
        hud.UpdateCooldown(0, 0);
        transform.GetPositionAndRotation(out startPosition, out startRotation);
        originalColor = sr.color;
    }

    void Update()
    {
        UpdateVertical();
        UpdateEquipment();
        UpdateAttack();
        UpdateInteract();
    }

    void FixedUpdate()
    {
        UpdateGrounded();
        UpdateHorizontal();
        UpdateForces();
    }

    void UpdateVertical()
    {
        if (Time.timeScale > 0)
        {
            if (jumpInput > 0)
            {
                jumpInput++;
            }
            if (jumpAction.WasPressedThisFrame())
            {
                jumpInput = 1;
            }
            if (jumpAction.WasReleasedThisFrame())
            {
                stoppedJump = true;
            }
        }
    }

    void UpdateEquipment()
    {
        if (Time.timeScale > 0 && equipmentAction.WasPerformedThisFrame())
        {
            if (inactiveWeapon != null)
            {
                (activeWeapon, inactiveWeapon) = (inactiveWeapon, activeWeapon);
                UpdateActiveEquipment();
            }
        }
    }

    internal void UpdateActiveEquipment()
    {
        float speed = animator.GetFloat("Speed");
        animator.runtimeAnimatorController = activeWeapon.animationController;
        animator.SetFloat("Speed", speed);
        hud.UpdateEquipment(activeWeapon, inactiveWeapon);
    }
    
    void UpdateAttack()
    {
        if (Time.timeScale > 0)
        {
            if (attackCooldown > 0)
            {
                attackCooldown -= Time.deltaTime;
                hud.UpdateCooldown(attackCooldown, attackCooldownMax);
            }
            if (attackCooldown <= 0 && attackAction.WasPerformedThisFrame())
            {
                if (activeWeapon.StartAttack(this))
                {
                    attackCooldown = attackCooldownMax = activeWeapon.cooldown;
                    hud.UpdateCooldown(attackCooldown, attackCooldownMax);
                }
            }
        }
    }

    internal void PerformAttack(int param)
    {
        if (Time.timeScale > 0)
        {
            activeWeapon.PerformAttack(this, param);
        }
    }

    void UpdateInteract()
    {
        if (Time.timeScale > 0 && interactAction.WasPerformedThisFrame())
        {
            if (chest != null)
            {
                GameStateManager.Instance.OpenChest(this, chest);
            }
        }
    }

    void UpdateGrounded()
    {
        int count = rb.GetContacts(contacts);
        maxYNormal = contacts.Take(count).Where(c => c.normalImpulse != 0).Select(c => c.normal).OrderBy(n => n.y).DefaultIfEmpty(new(0, -1)).Last();
        grounded = maxYNormal.y > minGroundNormalY;
    }

    void UpdateHorizontal()
    {
        int horizontal = (int)horizontalAction.ReadValue<float>();
        if (horizontal == 0)
        {
            horizontalState = HorizontalState.Idle;
            targetVelocityX = 0;
        }
        else
        {
            switch (horizontalState)
            {
                case HorizontalState.Idle:
                    horizontalState = HorizontalState.Run;
                    targetVelocityX = horizontal*runSpeed;
                    break;
                case HorizontalState.Run:
                    if (horizontal != direction)
                    {
                        targetVelocityX = horizontal*runSpeed;
                    }
                    if (runTime > dashRunTime)
                    {
                        horizontalState = HorizontalState.Dash;
                        targetVelocityX = horizontal*dashSpeed;
                    }
                    break;
                case HorizontalState.Dash:
                    if (horizontal != direction)
                    {
                        horizontalState = HorizontalState.Run;
                        targetVelocityX = horizontal*runSpeed;
                    }
                    break;
            }
        }
        float currentVelocityX = rb.velocity.x;
        if (horizontal != 0)
        {
            if (horizontal == direction && currentVelocityX != 0)
            {
                runTime += Time.fixedDeltaTime;
            }
            else
            {
                runTime = 0;
            }
        }
        else
        {
            runTime = 0;
        }
        direction = horizontal;
        float speed = Math.Abs(currentVelocityX);
        switch (direction)
        {
            case 1:
            {
                transform.localRotation = Constants.rightRotation;
                animator.SetFloat("Speed", (currentVelocityX < 0.5f ? 0.5f : speed)*(grounded ? 1 : 0.1f));
                break;
            }
            case -1:
            {
                transform.localRotation = Constants.leftRotation;
                animator.SetFloat("Speed", (currentVelocityX > -0.5f ? 0.5f : speed)*(grounded ? 1 : 0.1f));
                break;
            }
            case 0:
            {
                animator.SetFloat("Speed", (speed > 1 ? currentVelocityX*0.5f : 0)*(grounded ? 1 : 0.1f));
                break;
            }
        }
        animator.SetBool("Dash", horizontalState == HorizontalState.Dash);
    }

    void UpdateForces()
    {
        Vector2 groundTangent = grounded ? -Vector2.Perpendicular(maxYNormal) : new(1, 0);
        float currentVelocityX = Vector2.Dot(rb.velocity, groundTangent);
        if (jumpTicks > 0)
        {
            jumpTicks++;
        }
        if (jumpInput > 0 && grounded)
        {
            rb.AddForce(new(0, jumpSpeed*rb.mass), ForceMode2D.Impulse);
            jumpInput = 0;
            jumpTicks = 1;
        }
        else if (stoppedJump && rb.velocity.y > 0)
        {
            rb.AddForce(new(0, -rb.velocity.y*jumpSlowFactor*rb.mass), ForceMode2D.Impulse);
        }
        else if (jumpTicks == 0 && currentVelocityX != targetVelocityX)
        {
            float accelerationX = 0;
            if (currentVelocityX == 0)
            {
                accelerationX = targetVelocityX > 0 ? acceleration : -acceleration;
            }
            if (currentVelocityX > 0)
            {
                if (targetVelocityX <= 0)
                {
                    accelerationX = -Math.Min(1, (currentVelocityX-targetVelocityX)*4)*deceleration;
                }
                else if (grounded || Math.Abs(currentVelocityX) < aerialSpeed)
                {
                    accelerationX = Math.Min(1, (targetVelocityX-currentVelocityX)*4)*acceleration;
                }
            }
            if (currentVelocityX < 0)
            {
                if (targetVelocityX >= 0)
                {
                    accelerationX = Math.Min(1, (targetVelocityX-currentVelocityX)*4)*deceleration;
                }
                else if (grounded || Math.Abs(currentVelocityX) < aerialSpeed)
                {
                    accelerationX = -Math.Min(1, (currentVelocityX-targetVelocityX)*4)*acceleration;
                }
            }
            rb.AddForce(accelerationX*rb.mass*groundTangent, ForceMode2D.Force);
        }
        if (jumpInput > jumpBufferFrames)
        {
            jumpInput = 0;
        }
        if (jumpTicks > 1)
        {
            jumpTicks = 0;
        }
        stoppedJump = false;
        lastVelocity = rb.velocity;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        hud.UpdateHealth((float)health/maxHealth);
        FlashColor(flashTime);
        if (health <= 0)
        {
            Invoke(nameof(PlayerDied), 0.1f);
        }
        Debug.Log(health);
    }

    void PlayerDied()
    {
        GameStateManager.Instance.PlayerDied(this);
    }

    void FlashColor(float flashTime)
    {
        sr.color = flashColor;
        Invoke(nameof(ResetColor), flashTime);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newVelocity = rb.velocity;
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;
        float factor = Math.Max(normal.y, 0)*Math.Clamp(-lastVelocity.y*0.2f, 0, 5);
        newVelocity.x = Mathf.Lerp(newVelocity.x, lastVelocity.x, factor); 
        rb.velocity = newVelocity;
    }

    public enum HorizontalState {
            Idle,
            Run,
            Dash,
    }
}
