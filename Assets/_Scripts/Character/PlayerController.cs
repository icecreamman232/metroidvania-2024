using System;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public enum PlayerState
    {
        IDLE,
        RUNNING,
        JUMPING,
        CLIMBING,
        CROUCH,
        FROZEN,
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerState m_playerState;
        [SerializeField] private PlayerHorizontalMovement m_playerHorizontalMovement;
        [SerializeField] private PlayerJump m_playerJump;

        public PlayerState CurrentState => m_playerState;
        
        public void ChangeState(PlayerState newState)
        {
            m_playerState = newState;
        }

        public void FreezePlayer()
        {
            switch (m_playerState)
            {
                case PlayerState.RUNNING:
                    m_playerHorizontalMovement.StopRunning();
                    m_playerHorizontalMovement.ToggleAllow(false);
                    break;
                case PlayerState.JUMPING:
                    m_playerJump.StopJump();
                    m_playerJump.ToggleAllow(false);
                    break;
            }
            
            m_playerHorizontalMovement.ToggleAllow(false);
            m_playerState = PlayerState.FROZEN;
        }

        public void UnfreezePlayer()
        {
            m_playerState = PlayerState.IDLE;
        }
    }
}
