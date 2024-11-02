using UnityEngine;
using UnityEngine.Events;

namespace SGGames.Scripts.Dialogue
{
    /// <summary>
    /// Add this script to a game object which you want to be triggered if there's assigned choice got selected
    /// </summary>
    public class DialogueChoiceReceiver : MonoBehaviour
    {
        [SerializeField] private string m_choiceID;
        [SerializeField] private DialogueChoiceEvent m_choiceEvent;
        [SerializeField] private UnityEvent m_triggerEvent;

        protected virtual void OnEnable()
        {
            m_choiceEvent.AddListener(OnReceiveChoiceEvent);
        }
        
        protected virtual void OnDisable()
        {
            m_choiceEvent.AddListener(OnReceiveChoiceEvent);
        }
        
        protected virtual void OnReceiveChoiceEvent(string id)
        {
            if (m_choiceID != id) return;
            m_triggerEvent?.Invoke();
        }
    }
}
