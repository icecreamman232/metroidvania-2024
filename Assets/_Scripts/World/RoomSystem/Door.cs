using System;
using System.Collections;
using SGGames.Scripts.Managers;
using SGGames.Scripts.Player;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.World
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Door m_connectedDoor;
        [SerializeField] private Transform m_exitPoint;
        [SerializeField] private Room m_room;
        [SerializeField] private BoolEvent m_fadeScreenEvent;
        [SerializeField] private RoomVisitedEvent m_roomVisitedEvent;

        public string RoomID => m_room.RoomID;
        public Transform ExitPoint => m_exitPoint;
        public Room Room => m_room;
        
        private bool m_isProcess;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_isProcess) return;
            if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(OnTeleportToConnectDoor(other.gameObject));
            }
        }

        private IEnumerator OnTeleportToConnectDoor(GameObject player)
        {
            m_isProcess = true;
            //Fade screen out
            var playerController = player.GetComponent<PlayerController>();
            playerController.FreezePlayer();
            CameraFollowing.Instance.SetPermission(false); //Stop camera following
            m_fadeScreenEvent.Raise(true);
            yield return new WaitForSeconds(0.3f);
            
            m_room.ShowRoomMask();
            m_connectedDoor.Room.HideRoomMask();
            player.transform.position = m_connectedDoor.ExitPoint.position;
            m_roomVisitedEvent.Raise(m_connectedDoor.RoomID);
            CameraFollowing.Instance.SetCameraPosition(player.transform.position);
            CameraFollowing.Instance.SetPermission(true);
            //Fade screen in
            m_fadeScreenEvent.Raise(false);
            yield return new WaitForSeconds(0.3f);
            
            playerController.UnfreezePlayer();
            m_isProcess = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(m_exitPoint.position, Vector3.one * 0.3f);
            
            if (m_connectedDoor == null) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, m_connectedDoor.transform.position);
        }
    }
}

