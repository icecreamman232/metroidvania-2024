using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    /// <summary>
    /// Base class for player ability
    /// </summary>
    public class PlayerBehavior : MonoBehaviour
    {
        [SerializeField] protected bool m_isAllow;
        protected Controller2D m_controller;
        protected PlayerController m_playerController;
        
        protected virtual void Start()
        {
            m_playerController = GetComponent<PlayerController>();
            m_controller = GetComponent<Controller2D>();
        }

        public virtual void ToggleAllow(bool value)
        {
            m_isAllow = value;
        }
    }
}

