using SGGames.Scripts.ScriptableEvent;
using UnityEngine;
using UnityEngine.UI;

namespace SGGames.Scripts.UI
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private FloatEvent m_playerHealthEvent;
        [SerializeField] private Image m_healthBar;
        
        private void Start()
        {
            m_playerHealthEvent.AddListener(OnUpdateHealthBar);
        }

        private void OnDestroy()
        {
            m_playerHealthEvent.RemoveListener(OnUpdateHealthBar);
        }

        private void OnUpdateHealthBar(float value)
        {
            m_healthBar.fillAmount = value;
        }
    }
}
