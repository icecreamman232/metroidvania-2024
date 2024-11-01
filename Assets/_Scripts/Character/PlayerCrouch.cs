using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerCrouch : PlayerBehavior
    {
        [SerializeField] private BoxCollider2D m_playerCollider;
        [SerializeField] private Vector2 m_crouchOffset;
        [SerializeField] private Vector2 m_crouchSize;
        [SerializeField] private bool m_isCrouched;
        
        private Animator m_animator;
        private PlayerHorizontalMovement m_playerHorizontalMovement;
        private Vector2 m_originalOffset;
        private Vector2 m_originalSize;
        
        private readonly int m_triggerCrouchAnimParam = Animator.StringToHash("Crouch");
        private readonly int m_boolCrouchAnimParam = Animator.StringToHash("IsCrouching");
        
        protected override void Start()
        {
            m_isAllow = true;
            base.Start();
            m_playerHorizontalMovement = GetComponent<PlayerHorizontalMovement>();
            m_animator = GetComponentInChildren<Animator>();
            m_originalOffset = m_playerCollider.offset;
            m_originalSize = m_playerCollider.size;
        }

        private void Update()
        {
            HandleInput();
        }

        private bool CanCrouch()
        {
            if (!m_controller.CollisionInfos.CollideBelow) return false;

            if (m_playerController.CurrentState == PlayerState.CLIMBING)
            {
                return false;
            }

            return true;
        }

        private void HandleInput()
        {
            if (!m_isAllow) return;

            if (!CanCrouch()) return;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                if (m_isCrouched) return;

                m_isCrouched = true;
                m_playerController.ChangeState(PlayerState.CROUCH);
                SwitchToCrouch();
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                if(!m_isCrouched) return;
                
                m_isCrouched = false;
                m_playerController.ChangeState(m_controller.CollisionInfos.CollideBelow
                    ? PlayerState.IDLE
                    : PlayerState.JUMPING);

                SwitchToNormal();
            }
        }

        private void SwitchToCrouch()
        {
            m_playerCollider.offset = m_crouchOffset;
            m_playerCollider.size = m_crouchSize;
            
            m_controller.SetVelocity(Vector2.zero);
            m_playerHorizontalMovement.ToggleAllow(false);
            m_animator.SetTrigger(m_triggerCrouchAnimParam);
            m_animator.SetBool(m_boolCrouchAnimParam, true);
        }

        private void SwitchToNormal()
        {
            m_playerCollider.offset = m_originalOffset;
            m_playerCollider.size = m_originalSize;
            
            m_playerHorizontalMovement.ToggleAllow(true);
            m_animator.SetBool(m_boolCrouchAnimParam, false);
        }

        public void ExitCrouch()
        {
            m_isCrouched = false;
            SwitchToNormal();
        }
    }
}

