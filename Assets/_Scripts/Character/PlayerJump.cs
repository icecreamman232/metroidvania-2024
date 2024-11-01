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
        private PlayerCrouch m_playerCrouch;
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
            m_playerCrouch = GetComponent<PlayerCrouch>();
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
                if (m_playerController.CurrentState == PlayerState.CROUCH)
                {
                    m_playerCrouch.ExitCrouch();
                }
                
                m_controller.SetVerticalVelocity(0);
                m_isJumping = true;
                m_jumpStartTime = Time.time;
                m_coyoteTimer = 0;
                m_lastGroundY = transform.position.y;
                m_playerController.ChangeState(PlayerState.JUMPING);
            }


            if (Input.GetKeyUp(KeyCode.Space))
            {
                m_isJumping = false;
            }

            if (m_isJumping)
            {
                m_playerController.ChangeState(PlayerState.JUMPING);
                float jumpTime = Time.time - m_jumpStartTime;
                float jumpVelocity = m_jumpForce + (jumpTime * m_variableJumpHeightMultiplier);

                if (jumpVelocity >= m_jumpPeak)
                {
                    m_isJumping = false; // Stop jumping if max height reached
                    return;
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
