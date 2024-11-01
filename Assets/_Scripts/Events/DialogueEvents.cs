using System;
using UnityEngine;

namespace SGGames.Scripts.ScriptableEvent
{
    [CreateAssetMenu(menuName = "SGGames/Scriptable Event/Dialogue Event")]
    public class DialogueEvents : ScriptableObject
    {
        protected Action<string,string> m_listeners;
    
        public void AddListener(Action<string,string> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<string,string> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(string npcName, string dialogue)
        {
            m_listeners?.Invoke(npcName, dialogue);
        }
    }
}

