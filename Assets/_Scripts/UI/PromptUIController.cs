using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class PromptUIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        
        public void ShowPrompt()
        {
            m_canvasGroup.alpha = 1;
        }

        public void HidePrompt()
        {
            m_canvasGroup.alpha = 0;
        }
    }
}

