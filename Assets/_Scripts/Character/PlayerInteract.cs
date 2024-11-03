using SGGames.Scripts.Dialogue;
using SGGames.Scripts.World;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerInteract : PlayerBehavior
    {
        [Header("Dialogue Zone")]
        [SerializeField] private bool m_isInDialogueZone;
        [SerializeField] private DialogueTrigger m_dialogueTrigger;
        [Header("Door Zone")]
        [SerializeField] private bool m_isInDoorZone;
        [SerializeField] private DoorWithPrompt m_doorWithPrompt;
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
        }

        #region Dialogue
        public void ConnectDialogueTrigger(DialogueTrigger dialogueTrigger)
        {
            m_dialogueTrigger = dialogueTrigger;
            m_isInDialogueZone = true;
        }

        public void DisconnectDialogueTrigger()
        {
            m_dialogueTrigger = null;
            m_isInDialogueZone = false;
        }
        #endregion
        
        #region Door Zone
        public void ConnectDoor(DoorWithPrompt doorWithPrompt)
        {
            m_doorWithPrompt = doorWithPrompt;
            m_isInDoorZone = true;
        }

        public void DisconnectDoor()
        {
            m_doorWithPrompt = null;
            m_isInDoorZone = false;
        }
        #endregion

        private void Update()
        {
            if (!m_isAllow) return;

            if (m_isInDialogueZone && Input.GetKeyDown(KeyCode.E))
            {
                m_dialogueTrigger.TriggerDialogue(this.gameObject);
                DisconnectDialogueTrigger();
            }
            
            if (m_isInDoorZone && Input.GetKeyDown(KeyCode.E))
            {
                m_doorWithPrompt.TriggerDoor(this.gameObject);
                DisconnectDoor();
            }
        }
    }
}
