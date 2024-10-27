using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private bool m_noDamage;
    [SerializeField] private float m_delayBeforeDestroy;

    private SpriteRenderer m_renderer;

    protected override void Start()
    {
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        base.Start();
    }

    public void SetNoDamage(bool value)
    {
        m_noDamage = value;
    }

    public override void TakeDamage(int damage, GameObject instigator, float invulnerableDuration)
    {
        if (m_noDamage) return;
        base.TakeDamage(damage, instigator, invulnerableDuration);
    }

    protected override void Kill()
    {
        StartCoroutine(OnKilling());
    }

    private IEnumerator OnKilling()
    {
        m_isInvulnerable = true;
        OnDeath?.Invoke();
        m_renderer.enabled = false;
        yield return new WaitForSeconds(m_delayBeforeDestroy);
        this.gameObject.SetActive(false);
    }
}
