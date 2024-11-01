using System;
using System.Collections;
using SGGames.Scripts.ScriptableEvent;
using TMPro;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_dialogueText;
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private float m_speedToDisplay;
        [SerializeField] private bool m_isShowingDialogue;
        [SerializeField] private DialogueEvents m_dialogueEvents;
        [SerializeField] private string m_testDialogue;

        private WaitForSeconds m_waitForDisplayNextCharacter;

        private void Start()
        {
            m_waitForDisplayNextCharacter = new WaitForSeconds(m_speedToDisplay);
            m_dialogueEvents.AddListener(OnReceiveDialogueEvent);
        }

        private void OnReceiveDialogueEvent(string npcName, string dialogue)
        {
            ShowUI(dialogue);
        }

        private void OnDestroy()
        {
            m_dialogueEvents.RemoveListener(OnReceiveDialogueEvent);
        }

        private void ShowUI(string dialogueToShow)
        {
            m_canvasGroup.alpha = 1;
            ShowDialogue(dialogueToShow);
        }

        public void HideUI()
        {
            StopAllCoroutines();
            m_canvasGroup.alpha = 0;
        }
        
        private void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.T))
            {
                ShowDialogue(m_testDialogue);
            }
            #endif
        }
        
        
        private void ShowDialogue(string dialogue)
        {
            if (m_isShowingDialogue)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(OnShowDialogue(dialogue));
        }

        private IEnumerator OnShowDialogue(string dialogue)
        {
            m_isShowingDialogue = true;
            m_dialogueText.text = "";
            foreach (char letter in dialogue)
            {
                m_dialogueText.text += letter;
                yield return m_waitForDisplayNextCharacter;
            }
            m_isShowingDialogue = false;
        }
    }
}

