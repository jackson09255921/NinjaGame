using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health;
    public int damage;
    public float flashTime;

    private SpriteRenderer sr;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // 受傷閃鑠
    public void TakeDamage(int damage)
    {
        health -= damage;
        flashColor(flashTime);
        Debug.Log(health);
    }

    void flashColor(float flashTime)
    {
        sr.color = Color.red;
        Invoke("ResetColor", flashTime);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }
}
