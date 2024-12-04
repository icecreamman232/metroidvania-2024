using SGGames.Scripts.World;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerLadder : PlayerBehavior
    {
        [SerializeField] private Ladder m_curLadder;
        [SerializeField] private float m_climbSpeed = 5f;
        [SerializeField] private bool m_isClimbing;
        [SerializeField] private bool m_startedClimbing;
        [SerializeField] private bool m_isClimbUp;
        [SerializeField] private PlayerLadderState m_playerLadderState;

        private PlayerGunHandler m_playerGunHandler;
        private PlayerJump m_playerJump;
        private Animator m_animator;
        
        private int m_startClimbingAnimParam = Animator.StringToHash("StartClimbing");
        private int m_climbingAnimParam = Animator.StringToHash("Climbing");
        
        public bool HasStartedClimbing => m_startedClimbing;
        public bool IsClimbing => m_isClimbing;

        private enum PlayerLadderState
        {
            NONE,
            ENTER_FROM_TOP,
            ENTER_FROM_BOTTOM,
            ENTER_FROM_AIR,
        }
        
        protected override void Start()
        {
            base.Start();
            m_playerJump = GetComponent<PlayerJump>();
            m_playerGunHandler = GetComponent<PlayerGunHandler>();
            m_animator = GetComponentInChildren<Animator>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (m_curLadder != null) return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Ladder") && !m_startedClimbing)
            {
                m_curLadder = other.GetComponent<Ladder>();

                //Jump into ladder
                if ((m_controller.CollisionInfos.CollideLeft || m_controller.CollisionInfos.ColliderRight) && !m_controller.CollisionInfos.CollideBelow)
                {
                    m_playerLadderState = PlayerLadderState.ENTER_FROM_AIR;
                    m_controller.SetVerticalVelocity(0);
                    m_controller.SetGravityActive(false);
                    m_controller.RemoveLayerFromObstacles(1 << LayerMask.NameToLayer("Ladder"));
                }
                //Player runs into ladder from the bottom
                else if ((m_controller.CollisionInfos.CollideLeft || m_controller.CollisionInfos.ColliderRight) && m_controller.CollisionInfos.CollideBelow)
                {
                    m_playerLadderState = PlayerLadderState.ENTER_FROM_BOTTOM;
                    m_controller.RemoveLayerFromObstacles(1 << LayerMask.NameToLayer("Ladder"));
                }
                //Player is on the top of ladder
                else if (m_controller.CollisionInfos.CollideBelow)
                {
                    m_playerLadderState = PlayerLadderState.ENTER_FROM_TOP;
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            m_curLadder = null;
            m_controller.AddLayerToObstacles(1 << LayerMask.NameToLayer("Ladder"));
            
            m_startedClimbing = false;
            m_isClimbing = false;
            m_playerLadderState = PlayerLadderState.NONE;
            m_playerGunHandler.ToggleShootVertically(true);
            m_controller.SetVerticalVelocity(0);
            m_controller.SetGravityActive(true);
            m_playerController.ChangeState(m_controller.CollisionInfos.CollideBelow 
                                                        ? PlayerState.IDLE
                                                        : PlayerState.JUMPING);
            UpdateAnimator();
        }

        private void Update()
        {
            if (!m_isAllow) return;

            if (m_curLadder == null) { return; }

            if (!m_startedClimbing)
            {
                BeforeClimbing();
            }
            else
            {
                Climbing();
            }

            UpdateAnimator();
        }

        //Check if player is about to climbing or not.
        //If not we preserve current state of player to perform other actions
        private void BeforeClimbing()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (m_playerLadderState == PlayerLadderState.ENTER_FROM_BOTTOM
                    || m_playerLadderState == PlayerLadderState.ENTER_FROM_AIR
                    || (m_playerLadderState == PlayerLadderState.ENTER_FROM_TOP && !m_controller.CollisionInfos.CollideBelow))
                {
                    m_startedClimbing = true;
                    m_playerGunHandler.ToggleShootVertically(false);
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if ((m_playerLadderState == PlayerLadderState.ENTER_FROM_BOTTOM && m_controller.CollisionInfos.CollideBelow) 
                    || m_playerLadderState == PlayerLadderState.ENTER_FROM_TOP
                    || m_playerLadderState == PlayerLadderState.ENTER_FROM_AIR
                    || (m_playerLadderState == PlayerLadderState.NONE && m_controller.CollisionInfos.CollideBelow))
                {
                    m_startedClimbing = true;
                    m_playerGunHandler.ToggleShootVertically(false);
                }
            }
        }

        private void Climbing()
        {
            if(Input.GetKey(KeyCode.UpArrow))
            {
                m_isClimbUp = true;
                m_isClimbing = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                m_isClimbUp = false;
                m_isClimbing = true;
            }
            else
            {
                m_isClimbing = false;
            }
            
            if (m_isClimbing)
            {
                //While climbing we remove ladder from obstacle mask
                m_controller.RemoveLayerFromObstacles(1 << LayerMask.NameToLayer("Ladder"));
                m_controller.SetGravityActive(false);
                m_controller.SetVerticalVelocity(m_climbSpeed * (m_isClimbUp ? 1 : -1));
            }
            else
            {
                m_controller.SetVerticalVelocity(0);
            }
            m_playerController.ChangeState(PlayerState.CLIMBING);
        }

        private void UpdateAnimator()
        {
            m_animator.SetBool(m_startClimbingAnimParam, m_startedClimbing);
            m_animator.SetBool(m_climbingAnimParam, m_isClimbing);
        }
    }
}

