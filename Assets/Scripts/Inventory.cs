using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject kunaiPrefab;
    internal Transform shootPoint;
    Animator animator;
    bool equipment = true;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    internal void UpdateEquipment()
    {
        equipment = !equipment;
        animator.SetBool("Equipment", equipment);
    }

    internal void UpdateAttack() {
        if (!equipment)
        {
            GameObject kunaiInstance = Instantiate(kunaiPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(kunaiInstance, 0.5f);
            animator.SetTrigger("Attack");
        }
    }
}
