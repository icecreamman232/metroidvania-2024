using System.Collections;
using SGGames.Scripts.Managers;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerHealth : Health
    {
        [SerializeField] private bool m_godMode;
        [SerializeField] private FloatEvent m_updateHealthBarEvent;
        //[SerializeField] private ActionEvent m_deadEvent;
        [SerializeField] private float m_deadAnimDuration;
        [Header("Flick FX")]
        [SerializeField] private float m_delayBetween2Flick;
        [SerializeField] private Color m_flickColor;
        
        private Animator m_animator;
        private BoxCollider2D m_collider2D;
        private PlayerHorizontalMovement m_horizontalMovement;
        private SpriteRenderer m_spriteRenderer;
        private int m_deadAnimParam = Animator.StringToHash("Dead");
        
        protected override void Start()
        {
            m_horizontalMovement = GetComponent<PlayerHorizontalMovement>();
            m_collider2D = GetComponent <BoxCollider2D>();
            m_animator = GetComponentInChildren<Animator>();
            m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            base.Start();
            UpdateHealthBar();
        }

        public override void TakeDamage(int damage, GameObject instigator, float invulnerableDuration)
        {
            #if UNITY_EDITOR
                    if (m_godMode) return;
            #endif
            base.TakeDamage(damage, instigator, invulnerableDuration);
        }

        protected override IEnumerator OnInvulnerable(float duration)
        {
            m_isInvulnerable = true;
            var flickerStop = Time.time + duration;
            while (Time.time < flickerStop)
            {
                m_spriteRenderer.color = m_flickColor;
                yield return new WaitForSeconds(m_delayBetween2Flick);
                m_spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(m_delayBetween2Flick);
            }
            m_spriteRenderer.color = Color.white;
            m_isInvulnerable = false;
        }

        protected override void UpdateHealthBar()
        {
            m_updateHealthBarEvent.Raise(MathHelpers.Remap(m_currentHealth, 0, m_maxHealth, 0f, 1f));
            base.UpdateHealthBar();
        }

        protected override void Kill()
        {
            StartCoroutine(OnKillPlayer());
        }

        private IEnumerator OnKillPlayer()
        {
            m_collider2D.enabled = false;
            m_horizontalMovement.ToggleCamera(false);
            m_animator.SetTrigger(m_deadAnimParam);
            yield return new WaitForSeconds(m_deadAnimDuration);
            this.gameObject.SetActive(false);
            //m_deadEvent.Raise();
            //LevelManager.Instance.RevivePlayer();
        }
    }
}

