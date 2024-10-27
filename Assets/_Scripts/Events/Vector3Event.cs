using System;
using UnityEngine;

namespace SGGames.Scripts.ScriptableEvent
{
    [CreateAssetMenu(menuName = "SGGames/Scriptable Event/Vector3 Event")]
    public class Vector3Event : ScriptableObject
    {
        protected Action<Vector3> m_listeners;
    
        public void AddListener(Action<Vector3> addListener)
        {
            m_listeners += addListener;
        }

        public void RemoveListener(Action<Vector3> removeListener)
        {
            m_listeners -= removeListener;
        }

        public void Raise(Vector3 vector)
        {
            m_listeners?.Invoke(vector);
        }
    } 
}
