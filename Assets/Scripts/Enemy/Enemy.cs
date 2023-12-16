
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public float flashTime;
    public Color flashColor = Color.red;
    public Slider healthBar;

    internal Vector2 home;
    internal int health;
    SpriteRenderer sr;
    Color originalColor;

    protected virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        healthBar.value = 1;
        home = transform.position;
        health = maxHealth;
        originalColor = sr.color;
    }

    protected virtual void Update()
    {
        healthBar.transform.rotation = Constants.rightRotation;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.value = (float)health/maxHealth;
        FlashColor(flashTime);
        if (health <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    void FlashColor(float flashTime)
    {
        sr.color = flashColor;
        Invoke(nameof(ResetColor), flashTime);
    }

    void ResetColor()
    {
        sr.color = originalColor;
    }
}