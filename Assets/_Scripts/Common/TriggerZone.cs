using System;
using SGGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace SGGames.Scripts.Common
{
    public class TriggerZone : MonoBehaviour
    {
        [SerializeField] private LayerMask m_targetMask;
        [SerializeField] private UnityEvent m_onEnterEvent;
        [SerializeField] private UnityEvent m_onExitEvent;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask))
            {
                m_onEnterEvent?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (LayerManager.IsInLayerMask(other.gameObject.layer, m_targetMask))
            {
                m_onExitEvent?.Invoke();
            }
        }
    }
}

