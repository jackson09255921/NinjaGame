using System;
using System.Collections;
using System.Collections.Generic;
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
    public InputActionAsset actionAsset;
    InputActionMap defaultActionMap;
    InputAction jumpAction;
    InputAction horizontalAction;
    InputAction equipmentAction;
    InputAction attackAction;
    InputAction interactAction;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    HorizontalState horizontalState = HorizontalState.Idle;
    Vector2 lastVelocity;
    float targetVelocityX = 0;
    int direction = 0;
    float runTime = 0;
    bool jumped = false;
    ContactPoint2D[] contacts = new ContactPoint2D[16];
    bool equipment = true;

    void Awake()
    {
        defaultActionMap = actionAsset.FindActionMap("Default");
        jumpAction = defaultActionMap.FindAction("Jump");
        horizontalAction = defaultActionMap.FindAction("Horizontal");
        equipmentAction = defaultActionMap.FindAction("Equipment");
        attackAction = defaultActionMap.FindAction("Attack");
        interactAction = defaultActionMap.FindAction("Interact");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        defaultActionMap.Enable();
    }

    void OnDisable()
    {
        defaultActionMap.Disable();
    }

    void Start()
    {

    }

    void Update()
    {
        UpdateVertical();
        UpdateHorizontal();
        UpdateEquipment();
        UpdateAttack();
        UpdateInteract();
        UpdateShootPointPosition();
    }

    void UpdateVertical()
    {
        if (jumpAction.WasPerformedThisFrame())
        {
            jumped = true;
        }
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
                runTime += Time.deltaTime;
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
        Quaternion rotation = transform.rotation;
        Vector3 eulerAngles = rotation.eulerAngles;
        switch (direction)
        {
            case 1:
            {
                eulerAngles.y = 0;
                animator.SetFloat("Speed", speed);
                break;
            }
            case -1:
            {
                eulerAngles.y = 180;
                animator.SetFloat("Speed", speed);
                break;
            }
            case 0:
            {
                if (currentVelocityX > 0.5f)
                {
                    eulerAngles.y = 0;
                }
                if (currentVelocityX < -0.5f)
                {
                    eulerAngles.y = 180;
                }
                animator.SetFloat("Speed", speed > 0.5f ? speed*0.5f : 0);
                break;
            }
        }
        rotation.eulerAngles = eulerAngles;
        transform.rotation = rotation;
    }

    void UpdateEquipment()
    {
        if (equipmentAction.WasPerformedThisFrame())
        {
            equipment = !equipment;
            animator.SetBool("Equipment", equipment);
        }
    }
    
    void UpdateAttack()
    {
        if (attackAction.WasPerformedThisFrame())
        {
            animator.SetTrigger("Attack");
        }
    }
    
    void UpdateInteract()
    {
        if (interactAction.WasPerformedThisFrame())
        {

        }
    }

    void UpdateShootPointPosition()
    {
        // 更新射擊點位置
        //float xOffset = spriteRenderer.flipX ? -1f : 1f;
        //shootPoint.position = new Vector3(transform.position.x + xOffset, transform.position.y - 0.5f, transform.position.z);
        
        // 更新射擊點方向
        //Vector3 newRotation = spriteRenderer.flipX ? new Vector3(0, 180, 0) : Vector3.zero;
        //shootPoint.rotation = Quaternion.Euler(newRotation);
    }

    void FixedUpdate()
    {
        Vector2 maxYNormal = new(0, -1);
        int contactCount = rb.GetContacts(contacts);
        for (int i = 0; i < contactCount; ++i)
        {
            if (contacts[i].normal.y > maxYNormal.y)
            {
                maxYNormal = contacts[i].normal;
            }
        }
        bool isGrounded = maxYNormal.y > minGroundNormalY;
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
                else if (isGrounded || Math.Abs(currentVelocityX) < aerialSpeed)
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
                else if (isGrounded || Math.Abs(currentVelocityX) < aerialSpeed)
                {
                    accelerationX = -Math.Min(1, (currentVelocityX-targetVelocityX)*4)*acceleration;
                }
            }
            rb.AddForce(new(accelerationX*rb.mass, 0), ForceMode2D.Force);
        }
        if (jumped && isGrounded)
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
