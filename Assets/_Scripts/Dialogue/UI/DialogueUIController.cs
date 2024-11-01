using System.Collections;
using SGGames.Scripts.Dialogue;
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
        [SerializeField] private ActionEvent m_unfreezePlayerEvent;
        [SerializeField] private DialogueData m_currentDialogue;
        [SerializeField] private DialogueUIView m_dialogueUIView;
        [SerializeField] private bool m_canContinue;

        private int m_curLineIndex;
        private WaitForSeconds m_waitForDisplayNextCharacter;

        private void Start()
        {
            m_waitForDisplayNextCharacter = new WaitForSeconds(m_speedToDisplay);
            m_dialogueEvents.AddListener(OnReceiveDialogueEvent);
        }

        private void OnReceiveDialogueEvent(string npcName, DialogueData dialogue)
        {
            m_currentDialogue = dialogue;
            ShowUI();
        }

        private void OnDestroy()
        {
            m_dialogueEvents.RemoveListener(OnReceiveDialogueEvent);
        }

        private void Update()
        {
            if (m_currentDialogue == null) return;

            if (Input.GetKeyDown(KeyCode.Space) && m_canContinue)
            {
                Continue();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1) && m_canContinue)
            {
                ChooseChoice(0);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2) && m_canContinue)
            {
                ChooseChoice(1);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3) && m_canContinue)
            {
                ChooseChoice(2);
            }
        }

        private void Continue()
        {
            m_canContinue = false;
            m_curLineIndex++;
            if (m_curLineIndex >= m_currentDialogue.DialogueLines.Length)
            {
                ExitDialogue();
                return;
            }
            ShowDialogue(m_currentDialogue.DialogueLines[m_curLineIndex]);
        }

        private void ChooseChoice(int index)
        {
            m_currentDialogue = m_currentDialogue.DialogueLines[m_curLineIndex].Choices[index].ChoiceData;
            m_curLineIndex = 0; //Reset line index for new dialogue group
            ShowDialogue(m_currentDialogue.DialogueLines[m_curLineIndex]);
        }

        private void ExitDialogue()
        {
            HideUI();
            m_unfreezePlayerEvent.Raise();
        }

        private void ShowUI()
        {
            m_canvasGroup.alpha = 1;
            ShowDialogue(m_currentDialogue.DialogueLines[0]);
        }

        public void HideUI()
        {
            StopAllCoroutines();
            m_canvasGroup.alpha = 0;
        }
        
        private void ShowDialogue(DialogueLine line)
        {
            if (m_isShowingDialogue)
            {
                return;
            }
            StopAllCoroutines();
            ShowChoice(line);
            StartCoroutine(OnShowDialogue(line.Dialogues));
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
            m_canContinue = true;
        }

        private void ShowChoice(DialogueLine line)
        {
            //Reset choice view even if we dont have any choices
            m_dialogueUIView.ResetChoiceView();
            if (line.Choices == null || line.Choices.Length == 0) return;
            int choiceCount = 0;
            foreach (var choice in line.Choices)
            {
                m_dialogueUIView.ShowChoice(choiceCount,choice.ChoiceDialogue);
                choiceCount++;
            }
        }
    }
}

