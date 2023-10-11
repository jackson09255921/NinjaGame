using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject kunaiPrefab;
    Animator animator;  

    void Awake() {
        animator = GetComponent<Animator>();
    }

    internal void Shoot() {
        if (!animator.GetBool("Equipment"))
        {
            GameObject kunaiInstance = Instantiate(kunaiPrefab, shootPoint.position, shootPoint.rotation);
            Destroy(kunaiInstance, 0.5f);
        }
    }
}
