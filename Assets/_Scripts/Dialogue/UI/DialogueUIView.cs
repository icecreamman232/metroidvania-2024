using System;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class DialogueUIView : MonoBehaviour
    {
        [SerializeField] private GameObject m_continueBtn;
        [SerializeField] private DialogueChoiceUI[] m_choices;

        private void Start()
        {
            ResetChoiceView();
        }

        public void ResetChoiceView()
        {
            foreach (var choice in m_choices)
            {
                choice.HideChoice();
            }
        }

        public void ShowChoice(int choiceIndex, string choiceText)
        {
            m_choices[choiceIndex].ShowChoice(choiceText);
        }

        public void HideChoice(int choiceIndex)
        {
            m_choices[choiceIndex].HideChoice();
        }
        
        public void ShowContinueBtn()
        {
            m_continueBtn.SetActive(true);
        }

        public void HideContinueBtn()
        {
            m_continueBtn.SetActive(false);
        }
    }
}

