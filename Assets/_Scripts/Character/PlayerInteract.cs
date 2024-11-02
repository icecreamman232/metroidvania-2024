using SGGames.Scripts.Dialogue;
using UnityEngine;

namespace SGGames.Scripts.Player
{
    public class PlayerInteract : PlayerBehavior
    {
        [Header("Dialogue Zone")]
        [SerializeField] private bool m_isInDialogueZone;
        [SerializeField] private DialogueTrigger m_dialogueTrigger;
        
        protected override void Start()
        {
            base.Start();
            m_isAllow = true;
        }

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

        private void Update()
        {
            if (!m_isAllow) return;

            if (m_isInDialogueZone && Input.GetKeyDown(KeyCode.E))
            {
                m_dialogueTrigger.TriggerDialogue(this.gameObject);
                DisconnectDialogueTrigger();
            }
        }
    }
}
