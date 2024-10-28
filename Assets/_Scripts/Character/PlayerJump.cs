using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerJump : PlayerBehavior
    {
        [Header("New")]
        [SerializeField] private float m_jumpPeak;
        [SerializeField] private float m_jumpForce;
        [SerializeField] private float m_variableJumpHeightMultiplier;
        [SerializeField] private float m_coyoteTime;
        [SerializeField] private bool m_isJumping;
        
        private float m_jumpStartTime;
        private Animator m_animator;
        private int m_jumpAnimParam = Animator.StringToHash("Jumping");
        private float m_coyoteTimer;
        private float m_lastGroundY;
        
        public bool IsJumping => m_isJumping;
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
            m_animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (!m_isAllow)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && m_controller.CollisionInfos.CollideBelow
                && m_coyoteTimer <= m_coyoteTime && !m_isJumping)
            {
                m_controller.SetVerticalVelocity(0);
                m_isJumping = true;
                m_jumpStartTime = Time.time;
                m_coyoteTimer = 0;
                m_lastGroundY = transform.position.y;
            }


            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_isJumping = false;
            }

            if (m_isJumping)
            {
                float jumpTime = Time.time - m_jumpStartTime;
                float jumpVelocity = m_jumpForce + (jumpTime * m_variableJumpHeightMultiplier);

                if (jumpVelocity >= m_jumpPeak)
                {
                    Debug.Log($"CURRENT Y {transform.position.y} VEC {jumpVelocity}");
                    jumpVelocity = 0;
                    m_isJumping = false; // Stop jumping if max height reached
                }

                m_controller.SetVerticalVelocity(jumpVelocity);
            }
            
            
            if (!m_controller.CollisionInfos.CollideBelow)
            {
                m_coyoteTimer += Time.deltaTime;
            }
            else
            {
                m_coyoteTimer = 0;
            }
            
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            m_animator.SetBool(m_jumpAnimParam,m_controller.Velocity.y != 0 && m_controller.IsGravityActive);
        }
    }
}
