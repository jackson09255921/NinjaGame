using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
<<<<<<< Updated upstream
    public float moveSpeed = 5f;
=======
    public Animator animator;  // 新增Animator變量
    public float moveSpeed = 8f;
    public float moveSpeedUp = 12f;
>>>>>>> Stashed changes
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MoveCharacter();
        Jump();
    }

    void MoveCharacter()
    {
        float moveInputHorizontal = Input.GetAxis("Horizontal");
        float moveInputVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveInputHorizontal, moveInputVertical);
        movement.Normalize(); // Normalize to prevent faster diagonal movement

        // Move the character
<<<<<<< Updated upstream
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
=======
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        Jump();
>>>>>>> Stashed changes
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the character is grounded
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the character is no longer grounded
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}


