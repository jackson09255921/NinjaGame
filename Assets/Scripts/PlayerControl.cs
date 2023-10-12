using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float runSpeed = 5;
    public float aerialSpeed = 2;
    public float dashSpeed = 10;
    public float acceleration = 40;
    public float deceleration = 20;
    public float jumpSpeed = 15;
    public float dashRunTime = 2;
    public float minGroundNormalY = 0.65f;
    public Transform shootPoint;
    InputManager inputManager;
    InputAction jumpAction;
    InputAction horizontalAction;
    InputAction equipmentAction;
    InputAction attackAction;
    InputAction interactAction;
    Rigidbody2D rb;
    Animator animator;
    Inventory inventory;
    HorizontalState horizontalState = HorizontalState.Idle;
    Vector2 lastVelocity;
    float targetVelocityX = 0;
    int direction = 0;
    float runTime = 0;
    bool jumped = false;
    ContactPoint2D[] contacts = new ContactPoint2D[16];
    Vector2 maxYNormal;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inventory = GetComponent<Inventory>();
        inventory.shootPoint = shootPoint;
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
        if (Time.timeScale > 0 && jumpAction.WasPerformedThisFrame())
        {
            jumped = true;
        }
    }

    void UpdateEquipment()
    {
        if (Time.timeScale > 0 && equipmentAction.WasPerformedThisFrame())
        {
            inventory.UpdateEquipment();
        }
    }
    
    void UpdateAttack()
    {
        if (Time.timeScale > 0 && attackAction.WasPerformedThisFrame())
        {
            inventory.UpdateAttack();
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
        maxYNormal = contacts[0..count].Select(c => c.normal).OrderBy(n => n.y).DefaultIfEmpty(new(0, -1)).Last();
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
        Quaternion rotation = transform.localRotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        switch (direction)
        {
            case 1:
            {
                eulerAngles.y = 0;
                animator.SetFloat("Speed", (currentVelocityX < 0.5f ? 0.5f : speed)*(grounded ? 1 : 0.1f));
                break;
            }
            case -1:
            {
                eulerAngles.y = 180;
                animator.SetFloat("Speed", (currentVelocityX > -0.5f ? 0.5f : speed)*(grounded ? 1 : 0.1f));
                break;
            }
            case 0:
            {
                animator.SetFloat("Speed", (speed > 1 ? currentVelocityX*0.5f : 0)*(grounded ? 1 : 0.1f));
                break;
            }
        }
        rotation.eulerAngles = eulerAngles;
        transform.localRotation = rotation;
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
                if (targetVelocityX < currentVelocityX)
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
                if (targetVelocityX > currentVelocityX)
                {
                    accelerationX = Math.Min(1, (targetVelocityX-currentVelocityX)*4)*deceleration;
                }
                else if (grounded || Math.Abs(currentVelocityX) < aerialSpeed)
                {
                    accelerationX = -Math.Min(1, (currentVelocityX-targetVelocityX)*4)*acceleration;
                }
            }
            rb.AddForce(new(accelerationX*rb.mass, 0), ForceMode2D.Force);
        }
        if (jumped && grounded)
        {
            rb.AddForce(new(0, jumpSpeed*rb.mass), ForceMode2D.Impulse);
        }
        jumped = false;
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newVelocity = rb.velocity;
        for (int i = 0; i < collision.contactCount; ++i)
        {
            ContactPoint2D contact = collision.GetContact(i);
            Vector2 normal = contact.normal;
            Vector2 tangent = Vector2.Perpendicular(normal);
            Vector2 lastNormal = Vector2.Dot(lastVelocity, normal)*normal;
            Vector2 newNormal = Vector2.Dot(newVelocity, normal)*normal;
            if (lastNormal.sqrMagnitude > 25)
            {
                Vector2 lastTangent = Vector2.Dot(lastVelocity, tangent)*tangent;
                newVelocity = newNormal+lastTangent;
            }
            else
            {
                Vector2 currTangent = Vector2.Dot(rb.velocity, tangent)*tangent;
                newVelocity = newNormal+currTangent;
            }
        }
        rb.velocity = newVelocity;
    }

    public enum HorizontalState {
            Idle,
            Run,
            Dash,
    }
}
