using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SGGames.Scripts.World
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private string m_roomID;
        [SerializeField] private GameObject m_roomMask;
        [SerializeField] private BoxCollider2D m_roomCollider;

        public string RoomID => m_roomID;

        public BoxCollider2D RoomCollider => m_roomCollider;
        
        public void ShowRoomMask()
        {
            m_roomMask.SetActive(true);
        }

        public void HideRoomMask()
        {
            m_roomMask.SetActive(false);
        }
    }
}

