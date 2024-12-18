using System;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public enum PlayerFaceDirection
    {
        LEFT,
        RIGHT,
    }
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
        [SerializeField] private PlayerFaceDirection m_playerFaceDirection = PlayerFaceDirection.RIGHT; 
        [SerializeField] private ActionEvent m_unfreezePlayerEvent;
        [SerializeField] private PlayerHorizontalMovement m_playerHorizontalMovement;
        [SerializeField] private PlayerJump m_playerJump;
        [SerializeField] private PlayerInteract m_playerInteract;

        public PlayerState CurrentState => m_playerState;
        public PlayerFaceDirection CurrentFaceDirection => m_playerFaceDirection;

        private void Start()
        {
            m_unfreezePlayerEvent.AddListener(OnReceiveUnfreezePlayerEvent);
        }

        private void OnDestroy()
        {
            m_unfreezePlayerEvent.RemoveListener(OnReceiveUnfreezePlayerEvent);
        }

        public void ChangeState(PlayerState newState)
        {
            m_playerState = newState;
        }

        public void ChangeFaceDirection(PlayerFaceDirection newFaceDirection)
        {
            m_playerFaceDirection = newFaceDirection;
        }

        public void FreezePlayer()
        {
            m_playerJump.StopJump();
            m_playerJump.ToggleAllow(false);
            m_playerHorizontalMovement.StopRunning();
            m_playerHorizontalMovement.ToggleAllow(false);
            m_playerState = PlayerState.FROZEN;
        }

        public void UnfreezePlayer()
        {
            m_playerState = PlayerState.IDLE;
            m_playerJump.ToggleAllow(true);
            m_playerHorizontalMovement.ToggleAllow(true);
        }
        
        private void OnReceiveUnfreezePlayerEvent()
        {
            UnfreezePlayer();
        }
    }
}
