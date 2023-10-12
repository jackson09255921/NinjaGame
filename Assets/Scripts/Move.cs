using UnityEngine;

public class Move : MonoBehaviour
{
    public Animator animator;  // 新增Animator變量
    public float moveSpeed = 5f;
    public float moveSpeedUp = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalMove = 0f;
    private bool equipment = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();  // 初始化animator
    }

    void Update()
    {
        MoveCharacter();
        EquipmentChange();
        Jump();
        Attack();
    }

    void EquipmentChange()
    {
        equipment = !equipment;
        if (Input.GetKeyDown("c"))
        {
            animator.SetBool("Equipment", equipment);
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown("j"))
        {
            animator.SetTrigger("Attack");
        }
    }

    void MoveCharacter()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Move the character
        rb.velocity = new Vector2(horizontalMove, rb.velocity.y);

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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


