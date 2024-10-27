using SGGames.Scripts.Core;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    /// <summary>
    /// Base class for player ability
    /// </summary>
    public class PlayerBehavior : MonoBehaviour
    {
        protected Controller2D m_Controller;
        protected bool m_isAllow;
        
        protected virtual void Start()
        {
            m_Controller = GetComponent<Controller2D>();
        }

        public virtual void ToggleAllow(bool value)
        {
            m_isAllow = value;
        }
    }
}

