using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float m_maxHealth;
    [SerializeField] protected float m_currentHealth;

    protected bool m_isInvulnerable;

    public Action<float> OnHit;
    public Action OnDeath;

    protected virtual void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public virtual void ResetHealth()
    {
        m_currentHealth = m_maxHealth;
        m_isInvulnerable = false;
    }

    public virtual void TakeDamage(int damage, GameObject instigator, float invulnerableDuration)
    {
        if (damage <= 0) { return; }

        if (m_isInvulnerable) return;

        m_currentHealth -= damage;
        OnHit?.Invoke(m_currentHealth);
        UpdateHealthBar();
        
        if (m_currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            StartCoroutine(OnInvulnerable(invulnerableDuration));
        }

    }

    protected virtual void UpdateHealthBar()
    {
        
    }

    protected virtual IEnumerator OnInvulnerable(float duration)
    {
        m_isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        m_isInvulnerable = false;
    }

    protected virtual void Kill()
    {
        OnDeath?.Invoke();
        this.gameObject.SetActive(false);
    }
}
