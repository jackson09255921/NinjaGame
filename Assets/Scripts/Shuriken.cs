using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public float speed = 20;
    public float angularSpeed = 360;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 facing = transform.right;
        rb.velocity = facing*speed;
        rb.angularVelocity = facing.x*angularSpeed;
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
