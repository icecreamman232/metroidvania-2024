using System;
using SGGames.Scripts.World;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerLadder : PlayerBehavior
    {
        [SerializeField] private Ladder m_curLadder;
        [SerializeField] private float m_climbSpeed = 5f;
        [SerializeField] private bool m_isClimbing;

        private bool m_startedClimbing;
        private bool m_isClimbUp;
        private PlayerJump m_playerJump;
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
            m_playerJump = GetComponent<PlayerJump>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ladder"))
            {
                m_curLadder = other.GetComponent<Ladder>();
                if (!m_controller.CollisionInfos.CollideBelow)
                {
                    m_controller.SetGravityActive(false);
                    m_controller.SetVelocity(Vector2.zero);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ladder") && !m_startedClimbing)
            {
                if (!m_controller.CollisionInfos.CollideBelow)
                {
                    m_controller.SetGravityActive(false);
                    //m_controller.SetVelocity(Vector2.zero);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ladder")
                && m_curLadder.gameObject == other.gameObject)
            {
                m_startedClimbing = false;
                m_isClimbing = false;
                if (GetHeadYPosition() > m_curLadder.RightSnapPoint.position.y)
                {
                    transform.position = m_controller.Velocity.x < 0 
                        ? m_curLadder.LeftSnapPoint.position 
                        : m_curLadder.RightSnapPoint.position;
                }
                
                m_controller.SetVerticalVelocity(0);
                m_controller.SetGravityActive(true);
                m_curLadder = null;
                
            }
        }

        private void Update()
        {
            if (!m_isAllow) return;

            if (m_curLadder == null) return;

            if (Input.GetKey(KeyCode.UpArrow))
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
                m_startedClimbing = true;
                m_controller.SetGravityActive(false);
                m_controller.SetVerticalVelocity(m_climbSpeed * (m_isClimbUp ? 1 : -1));
            }
            else
            {
                m_controller.SetVerticalVelocity(0);
            }
        }

        private float GetHeadYPosition()
        {
            return transform.position.y + 0.7f;
        }
    }
}

