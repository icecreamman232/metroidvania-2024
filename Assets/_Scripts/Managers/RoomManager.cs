using System.Collections.Generic;
using SGGames.Scripts.ScriptableEvent;
using SGGames.Scripts.World;
using UnityEngine;

namespace SGGames.Scripts.Managers
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private int m_defaultRoomIndex;
        [SerializeField] private RoomVisitedEvent m_RoomVisitedEvent;
        [SerializeField] private Room[] m_rooms;
        [SerializeField] private List<string> m_visitedRooms;

        private void Start()
        {
            m_visitedRooms = new List<string>();
            m_RoomVisitedEvent.AddListener(OnVisitNewRoom);
            CameraController.Instance.SetRoomCollider(m_rooms[m_defaultRoomIndex].RoomCollider);
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

