
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public float flashTime;
    public Color flashColor = Color.red;
    public Slider healthBar;

    internal int health;
    SpriteRenderer sr;
    Color originalColor;

    protected virtual void Awake()
    {
        health = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        FlashColor(flashTime);
        if (health <= 0)
        {
            Destroy(gameObject, 0.1f);
        }
        Debug.Log(health);
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