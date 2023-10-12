using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject kunaiPrefab;
    public GameObject shurikenPrefab;
    internal Transform shootPoint;
    Animator animator;
    bool equipment = true;
    ContactFilter2D contactFilter;
    Collider2D[] overlaps = new Collider2D[16];

    void Awake() {
        animator = GetComponent<Animator>();
        contactFilter = new();
        contactFilter.NoFilter();
        contactFilter.useTriggers = false;
    }

    internal void UpdateEquipment()
    {
        equipment = !equipment;
        animator.SetBool("Equipment", equipment);
    }

    internal void UpdateAttack() {
        if (!equipment)
        {
            int count = Physics2D.OverlapCircle(shootPoint.position, 0.5f, contactFilter, overlaps);
            if (overlaps[0..count].All(c => !c.CompareTag("Ground")))
            {
                GameObject shurikenInstance = Instantiate(shurikenPrefab, shootPoint.position, shootPoint.rotation);
                Destroy(shurikenInstance, 0.5f);
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            animator.SetTrigger("Attack");
        }
    }
}
