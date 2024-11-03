
using System;
using SGGames.Scripts.Player;
using UnityEngine;

namespace SGGames.Scripts.World
{
    public class DoorWithPrompt : Door
    {
        private bool m_isInTheZone;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (m_isProcess) return;
            var playerInteract = other.GetComponent<PlayerInteract>();
            playerInteract.ConnectDoor(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerInteract = other.GetComponent<PlayerInteract>();
                playerInteract.DisconnectDoor();
            }
        }

        public void TriggerDoor(GameObject player)
        {
            StartCoroutine(OnTeleportToConnectDoor(player));
        }
    }
}

