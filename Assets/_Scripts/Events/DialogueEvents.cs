using System;
using SGGames.Scripts.Dialogue;
using UnityEngine;

namespace SGGames.Scripts.ScriptableEvent
{
    [CreateAssetMenu(menuName = "SGGames/Scriptable Event/Dialogue Event")]
    public class DialogueEvents : ScriptableObject
    {
        protected Action<string,DialogueData> m_listeners;
    
        public void AddListener(Action<string,DialogueData> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<string,DialogueData> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(string npcName, DialogueData dialogue)
        {
            m_listeners?.Invoke(npcName, dialogue);
        }
    }
}

