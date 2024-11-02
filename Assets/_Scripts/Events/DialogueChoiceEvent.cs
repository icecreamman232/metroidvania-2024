using System;
using UnityEngine;

namespace SGGames.Scripts.Dialogue
{
    [CreateAssetMenu(menuName = "SGGames/Scriptable Event/Choice Event")]
    public class DialogueChoiceEvent : ScriptableObject
    {
        protected Action<string> m_listeners;
    
        public void AddListener(Action<string> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<string> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(string choiceID)
        {
            m_listeners?.Invoke(choiceID);
        }
    }
}

