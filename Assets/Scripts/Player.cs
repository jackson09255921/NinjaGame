using System;
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
    public Weapon[] weapons;

    // Components
    internal Rigidbody2D rb;
    internal Animator animator;

    // Inputs
    InputManager inputManager;
    InputAction jumpAction;
    InputAction horizontalAction;
    InputAction equipmentAction;
    InputAction attackAction;
    InputAction interactAction;

    // Weapon fields
    int slot;

    // Movement fields
    internal HorizontalState horizontalState = HorizontalState.Idle;
    Vector2 lastVelocity;
    float targetVelocityX = 0;
    int direction = 0;
    float runTime = 0;
    int jumped = 0;
    bool stoppedJump = false;
    readonly ContactPoint2D[] contacts = new ContactPoint2D[16];
    Vector2 maxYNormal;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        inputManager = InputManager.Instance;
        jumpAction = inputManager.FindAction("Jump");
        horizontalAction = inputManager.FindAction("Horizontal");
        equipmentAction = inputManager.FindAction("Equipment");
        attackAction = inputManager.FindAction("Attack");
        interactAction = inputManager.FindAction("Interact");
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
            if (jumped > 0)
            {
                jumped++;
            }
            if (jumpAction.WasPressedThisFrame())
            {
                jumped = 1;
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
            int nextSlot = (slot+1)%weapons.Length;
            if (weapons[nextSlot] != null)
            {
                slot = nextSlot;
                float speed = animator.GetFloat("Speed");
                animator.runtimeAnimatorController = weapons[slot].animationController;
                animator.SetFloat("Speed", speed);
            }
        }
    }
    
    void UpdateAttack()
    {
        if (Time.timeScale > 0 && attackAction.WasPerformedThisFrame())
        {
            weapons[slot].PerformAttack(this);
        }
    }

    void UpdateInteract()
    {
        if (Time.timeScale > 0 && interactAction.WasPerformedThisFrame())
        {

        }
    }

    void UpdateGrounded()
    {
        int count = rb.GetContacts(contacts);
        maxYNormal = contacts.Take(count).Select(c => c.normal).OrderBy(n => n.y).DefaultIfEmpty(new(0, -1)).Last();
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
        float currentVelocityX = rb.velocity.x;
        if (currentVelocityX != targetVelocityX)
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
            rb.AddForce(accelerationX*rb.mass*(grounded ? -Vector2.Perpendicular(maxYNormal) : new(1, 0)), ForceMode2D.Force);
        }
        if (jumped > 0 && grounded)
        {
            rb.AddForce(new(0, jumpSpeed*rb.mass), ForceMode2D.Impulse);
            jumped = 0;
        }
        if (jumped > jumpBufferFrames)
        {
            jumped = 0;
        }
        if (stoppedJump && rb.velocity.y > 0)
        {
            rb.AddForce(new(0, -rb.velocity.y*jumpSlowFactor*rb.mass), ForceMode2D.Impulse);
        }
        stoppedJump = false;
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newVelocity = rb.velocity;
        ContactPoint2D contact = collision.GetContact(0);
        Vector2 normal = contact.normal;
        float factor = Math.Max(normal.y, 0)*Math.Clamp(-lastVelocity.y, 0, 5)*0.2f;
        newVelocity.x = Mathf.Lerp(newVelocity.x, lastVelocity.x, factor); 
        rb.velocity = newVelocity;
    }

    public enum HorizontalState {
            Idle,
            Run,
            Dash,
    }
}
