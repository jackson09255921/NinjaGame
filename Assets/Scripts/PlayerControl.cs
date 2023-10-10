using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float runSpeed = 5;
    public float dashSpeed = 10;
    public float acceleration = 40;
    public float deceleration = 20;
    public float jumpSpeed = 15;
    public float dashRunTime = 2;
    public float minGroundNormalY = 0.65f;
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
    public float targetVelocityX = 0;
    public int direction = 0;
    public float runTime = 0;
    public bool isGrounded = false;
    ContactPoint2D[] contacts = new ContactPoint2D[16];
    bool equipment = true;

    public Transform shootPoint;

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
        defaultActionMap.Enable();
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
        isGrounded = false;
        int contactCount = rb.GetContacts(contacts);
        for (int i = 0; i < contactCount; ++i)
        {
            if (contacts[i].normal.y > minGroundNormalY)
            {
                isGrounded = true;
            }
        }
        if (isGrounded && jumpAction.WasPerformedThisFrame())
        {
            rb.velocity = new(rb.velocity.x, jumpSpeed);
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
        if (currentVelocityX != targetVelocityX)
        {
            float nextVelocityX = currentVelocityX;
            if (currentVelocityX == 0)
            {
                nextVelocityX = targetVelocityX > 0 ?
                Math.Min(acceleration*Time.deltaTime, targetVelocityX) :
                Math.Max(-acceleration*Time.deltaTime, targetVelocityX);
            }
            if (currentVelocityX > 0)
            {
                if (targetVelocityX < currentVelocityX)
                {
                    nextVelocityX = Math.Max(currentVelocityX-deceleration*Time.deltaTime, 0);
                }
                else if (isGrounded)
                {
                    nextVelocityX = Math.Min(currentVelocityX+acceleration*Time.deltaTime, targetVelocityX);
                }
            }
            if (currentVelocityX < 0)
            {
                if (targetVelocityX > currentVelocityX)
                {
                    nextVelocityX = Math.Min(currentVelocityX+deceleration*Time.deltaTime, 0);
                }
                else if (isGrounded)
                {
                    nextVelocityX = Math.Max(currentVelocityX-acceleration*Time.deltaTime, targetVelocityX);
                }
            }
            rb.velocity = new(nextVelocityX, rb.velocity.y);
        }
        currentVelocityX = rb.velocity.x;
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

        // 角色翻轉
        direction = horizontal;
        lastVelocity = rb.velocity;
        if (direction > 0)
        {
            spriteRenderer.flipX = false;

        }
        if (direction < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat("Speed", Mathf.Abs(currentVelocityX));
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

    void UpdateShootPointPosition()
    {
        // 更新射擊點位置
        float xOffset = spriteRenderer.flipX ? -1f : 1f;
        shootPoint.position = new Vector3(transform.position.x + xOffset, transform.position.y - 0.5f, transform.position.z);
        
        // 更新射擊點方向
        Vector3 newRotation = spriteRenderer.flipX ? new Vector3(0, 180, 0) : Vector3.zero;
        shootPoint.rotation = Quaternion.Euler(newRotation);
    }
}
