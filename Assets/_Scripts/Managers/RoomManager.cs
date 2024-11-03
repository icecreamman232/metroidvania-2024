using System.Collections.Generic;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private RoomVisitedEvent m_RoomVisitedEvent;
        [SerializeField] private List<string> m_visitedRooms;

        private void Start()
        {
            m_visitedRooms = new List<string>();
            m_RoomVisitedEvent.AddListener(OnVisitNewRoom);
        }

        private void OnDestroy()
        {
            m_RoomVisitedEvent.RemoveListener(OnVisitNewRoom);
        }

        private void OnVisitNewRoom(string roomID)
        {
            if(m_visitedRooms.Contains(roomID)) return;
            m_visitedRooms.Add(roomID);
        }
    }
}

