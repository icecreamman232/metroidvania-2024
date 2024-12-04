using System.Collections;
using SGGames.Scripts.Managers;
using SGGames.Scripts.Player;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.World
{
    public class Door : MonoBehaviour
    {
        [SerializeField] protected Door m_connectedDoor;
        [SerializeField] protected Transform m_exitPoint;
        [SerializeField] protected Room m_room;
        [SerializeField] protected BoolEvent m_fadeScreenEvent;
        [SerializeField] protected RoomVisitedEvent m_roomVisitedEvent;

        public string RoomID => m_room.RoomID;
        public Transform ExitPoint => m_exitPoint;
        public Room Room => m_room;
        
        protected bool m_isProcess;
        
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (m_isProcess) return;
            if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                StartCoroutine(OnTeleportToConnectDoor(other.gameObject));
            }
        }

        protected virtual IEnumerator OnTeleportToConnectDoor(GameObject player)
        {
            m_isProcess = true;
            //Fade screen out
            var playerController = player.GetComponent<PlayerController>();
            playerController.FreezePlayer();
            CameraController.Instance.SetPermission(false); //Stop camera following
            m_fadeScreenEvent.Raise(true);
            yield return new WaitForSeconds(0.3f);
            
            m_room.ShowRoomMask();
            m_connectedDoor.Room.HideRoomMask();
            
            
            player.transform.position = m_connectedDoor.ExitPoint.position;
            m_roomVisitedEvent.Raise(m_connectedDoor.RoomID);
            
            CameraController.Instance.SetRoomCollider(m_connectedDoor.Room.RoomCollider);
            CameraController.Instance.SetCameraPosition(player.transform.position);
            CameraController.Instance.SetPermission(true);
            
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

