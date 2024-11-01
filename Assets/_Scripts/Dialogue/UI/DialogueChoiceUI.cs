using TMPro;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class DialogueChoiceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_choiceText;

        public void ShowChoice(string text)
        {
            this.gameObject.SetActive(true);
            m_choiceText.text = text;
        }

        public void HideChoice()
        {
            this.gameObject.SetActive(false);
        }
    }
}
