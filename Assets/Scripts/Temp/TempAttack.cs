using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempAttack : MonoBehaviour
{
    // 暫時嵌入在敵人與玩家身上
    // Script 都在 temp script profile 中

    // Inputs
    InputManager inputManager;
    InputAction jumpAction;
    InputAction horizontalAction;
    InputAction equipmentAction;
    InputAction attackAction;
    InputAction interactAction;


    public int damage;
    public float startTime;
    public float durationTime;
    

    private Animator anim;
    private PolygonCollider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        jumpAction = inputManager.FindAction("Default/Jump");
        horizontalAction = inputManager.FindAction("Default/Horizontal");
        equipmentAction = inputManager.FindAction("Default/Equipment");
        attackAction = inputManager.FindAction("Default/Attack");
        interactAction = inputManager.FindAction("Default/Interact");


        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        collider2D = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (attackAction.WasPerformedThisFrame())
        {
            
            StartCoroutine(startAttack());
        }
    }

    IEnumerator startAttack()
    {
        yield return new WaitForSeconds(startTime);
        collider2D.enabled = true;
        StartCoroutine(disableHitBox());
    }

    IEnumerator disableHitBox()
    {
        yield return new WaitForSeconds(durationTime);
        collider2D.enabled = false;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
