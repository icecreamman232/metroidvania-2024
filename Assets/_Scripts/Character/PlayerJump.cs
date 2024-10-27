using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerJump : PlayerBehavior
    {
        [SerializeField] private float m_jumpHeight;
        [SerializeField] private float m_timeToJumpPeak;
        [SerializeField] private float m_jumpVelocity;
        [SerializeField] private float m_jumpAcceleration;
        [SerializeField] private float m_coyoteTime;

        //private PlayerSoundBank m_soundBank;
        private Animator m_animator;

        private int m_jumpAnimParam = Animator.StringToHash("Jumping");
        private bool m_isJumping;
        //private float m_lastJumpY;
        private float m_coyoteTimer;
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
            m_animator = GetComponentInChildren<Animator>();
            //m_soundBank = GetComponent<PlayerSoundBank>();
            ComputeJumpParams();
        }

        private void Update()
        {
            if (!m_isAllow)
            {
                return;
            }

            if (!m_Controller.CollisionInfos.CollideBelow)
            {
                m_coyoteTimer += Time.deltaTime;
            }
            else
            {
                m_coyoteTimer = 0;
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && m_Controller.CollisionInfos.CollideBelow
                && m_coyoteTimer <= m_coyoteTime)
            {
                m_Controller.SetVerticalVelocity(m_jumpVelocity);
                //m_soundBank.PlayJumpSFX();
                //m_isJumping = true;
                m_coyoteTimer = 0;
            }
            
            UpdateAnimator();
        }

        private void ComputeJumpParams()
        {
            m_timeToJumpPeak = Mathf.Sqrt(2 * m_jumpHeight / Mathf.Abs(m_Controller.Gravity));
            m_jumpVelocity = Mathf.Abs(m_Controller.Gravity) * m_timeToJumpPeak;
        }

        private void UpdateAnimator()
        {
            m_animator.SetBool(m_jumpAnimParam,m_Controller.Velocity.y != 0 && m_Controller.IsGravityActive);
        }
    }
}
