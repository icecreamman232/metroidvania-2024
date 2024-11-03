using System;
using UnityEngine;

namespace SGGames.Scripts.ScriptableEvent
{
    [CreateAssetMenu(menuName = "SGGames/Scriptable Event/Room Visited Event")]
    public class RoomVisitedEvent : ScriptableObject
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

        public void Raise(string roomID)
        {
            m_listeners?.Invoke(roomID);
        }
    }
}

