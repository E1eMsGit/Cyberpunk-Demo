using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Damagable : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    public float invencibleTime = 0.1f;

    public UnityEvent OnDamage;
    public UnityEvent OnFinishDamage;
    public UnityEvent OnDeath;

    private bool canTakeDamage = true;

    private SpriteRenderer spriteRenderer;

    protected void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int amount)
    {
        if (!canTakeDamage)
        {
            return;
        }

        canTakeDamage = false;
        currentHealth -= amount;
        OnDamage.Invoke();

        if (currentHealth <= 0)
        {
            OnDeath.Invoke();
            Death();
            return;
        }

        StartCoroutine(BlinkSprite());
    }

    IEnumerator BlinkSprite()
    {
        float timer = 0;
        Color defaultColor = spriteRenderer.color;
        while (timer < invencibleTime)
        {
            spriteRenderer.color = Color.clear;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = defaultColor;
            yield return new WaitForSeconds(0.05f);
            timer += 0.1f;
        }

        spriteRenderer.color = defaultColor;
        canTakeDamage = true;
        OnFinishDamage.Invoke();
    }

    public abstract void Death();
}
