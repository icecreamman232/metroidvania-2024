using System.Collections;
using SGGames.Scripts.Managers;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerGunHandler : PlayerBehavior
    {
        [SerializeField] private ObjectPooler m_bulletPooler;
        
        [SerializeField] private float m_delayBetween2Shot;
        [SerializeField] private Transform m_shootPivot;
        [SerializeField] private Vector2 m_aimDirection = Vector2.right;
        [SerializeField] private Vector2 m_leftShootOffset;
        [SerializeField] private Vector2 m_rightShootOffset;
        [SerializeField] private Vector2 m_upShootOffset;
        [SerializeField] private Vector2 m_downShootOffset;


        private bool m_canShootVertical;
        private bool m_canShootAtRest;
        
        private bool m_isDelay;
        private PlayerHorizontalMovement m_horizontalMovement;
        private Animator m_animator;
        private readonly int m_shootHorizontalAnim = Animator.StringToHash("ShootHorizontal");
        private readonly int m_shootVerticalAnim = Animator.StringToHash("ShootVertical");

        public void ToggleShootVertically(bool toggle)
        {
            m_canShootVertical = toggle;
        }
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
            m_canShootVertical = true;
            m_animator = GetComponentInChildren<Animator>();
            m_horizontalMovement = GetComponent<PlayerHorizontalMovement>();
        }

        private bool CanShoot()
        {
            if (m_playerController.CurrentState == PlayerState.CROUCH)
            {
                return false;
            }
            
            if (m_isDelay) return false;
            
            return true;
        }

        private void Update()
        {
            if (!m_isAllow) return;
            
            if (Input.GetKeyDown(KeyCode.F))
            {

                if (!CanShoot()) return;
                
                var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (input.x > 0)
                {
                    m_aimDirection = Vector2.right;
                    m_shootPivot.localPosition = m_rightShootOffset;
                    Shoot();
                }
                else if (input.x < 0)
                {
                    m_aimDirection = Vector2.left;
                    m_shootPivot.localPosition = m_leftShootOffset;
                    Shoot();
                }
                
                if (input.y > 0 && m_canShootVertical)
                {
                    m_aimDirection = Vector2.up;
                    m_shootPivot.localPosition = m_upShootOffset;
                    Shoot();
                }
                else if (input.y < 0 && m_canShootVertical)
                {
                    m_aimDirection = Vector2.down;
                    m_shootPivot.localPosition = m_downShootOffset;
                    Shoot();
                }

                if (input is { x: 0, y: 0 })
                {
                    m_aimDirection = m_horizontalMovement.IsFlip ? Vector2.left : Vector2.right;
                    m_shootPivot.localPosition = m_horizontalMovement.IsFlip ? m_leftShootOffset : m_rightShootOffset;
                    Shoot();
                }
            }
        }
        
        private void Shoot()
        {
            var bulletGO = m_bulletPooler.GetPooledGameObject();
            var bullet = bulletGO.GetComponent<PlayerBullet>();
            bullet.Spawn(m_shootPivot.transform.position, m_aimDirection);

            if (m_aimDirection.y > 0)
            {
                m_animator.SetTrigger(m_shootVerticalAnim);
            }
            else if(m_aimDirection.x != 0)
            {
                m_animator.SetTrigger(m_shootHorizontalAnim);
            }
            
            //Reset aim direction after shot
            m_aimDirection = Vector2.zero;
            
            
            StartCoroutine(OnDelayBetween2Shot());
        }

        private IEnumerator OnDelayBetween2Shot()
        {
            m_isDelay = true;
            yield return new WaitForSeconds(m_delayBetween2Shot);
            m_isDelay = false;
        }
    }
}

