using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject kunaiPrefab;
    Animator animator;  
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    void Update(){
        if (Input.GetKeyDown("space") && !animator.GetBool("Equipment")){
            Shoot();
        }
    }

   void Shoot(){
        GameObject kunaiInstance = Instantiate(kunaiPrefab, firePoint.position, firePoint.rotation);
        Destroy(kunaiInstance, 0.5f);
   }
}
