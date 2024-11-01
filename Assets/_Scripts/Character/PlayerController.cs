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
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerState m_playerState;

        public PlayerState CurrentState => m_playerState;
        
        public void ChangeState(PlayerState newState)
        {
            m_playerState = newState;
        }
    }
}
