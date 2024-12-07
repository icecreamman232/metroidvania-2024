using System.Collections;
using SGGames.Scripts.Player;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;
using UnityEngine.Events;

namespace SGGames.Scripts.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private string m_npcName;
        [SerializeField] private DialogueEvents m_DialogueEvents;
        [SerializeField] private DialogueData m_startDialogue;
        [SerializeField] private UnityEvent m_onDialogueTriggered;
        [SerializeField] private UnityEvent m_onDialogueStarted;
        [SerializeField] private UnityEvent m_onDialogueDisconnect;

        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerInteract = other.GetComponent<PlayerInteract>();
                playerInteract.ConnectDialogueTrigger(this);
                m_onDialogueTriggered?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var playerInteract = other.GetComponent<PlayerInteract>();
                playerInteract.DisconnectDialogueTrigger();
                m_onDialogueDisconnect?.Invoke();
            }
        }

        public void TriggerDialogue(GameObject player)
        {
            StartCoroutine(OnTriggerDialogue(player.gameObject));
        }
        

        private IEnumerator OnTriggerDialogue(GameObject player)
        {
            var playerController = player.GetComponent<PlayerController>();
            playerController.FreezePlayer();
            yield return new WaitUntil(() => playerController.CurrentState == PlayerState.FROZEN);
            m_onDialogueStarted?.Invoke();
            m_DialogueEvents.Raise(m_npcName, m_startDialogue);
        }
    }
}

