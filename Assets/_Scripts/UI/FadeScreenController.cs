using System.Collections;
using SGGames.Scripts.Managers;
using SGGames.Scripts.ScriptableEvent;
using UnityEngine;

namespace SGGames.Scripts.UI
{
    public class FadeScreenController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup m_canvasGroup;
        [SerializeField] private BoolEvent m_fadeScreenEvent;
        [SerializeField] private float m_fadeDuration;

        private bool m_isFading = false;

        private void Awake()
        {
            m_fadeScreenEvent.AddListener(OnFadeScreen);
        }

        private void OnDestroy()
        {
            m_fadeScreenEvent.RemoveListener(OnFadeScreen);
        }

        private void OnFadeScreen(bool isfadeOut)
        {
            if (m_isFading) return;
            if (isfadeOut)
            {
                StartCoroutine(OnFadeOut());
            }
            else
            {
                StartCoroutine(OnFadeIn());
            }
        }

        private IEnumerator OnFadeIn()
        {
            m_isFading = true;
            var timer = m_fadeDuration;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer,0,m_fadeDuration,0,1);
                yield return null;
            }

            m_canvasGroup.alpha = 0;
            m_isFading = false;
        }

        private IEnumerator OnFadeOut()
        {
            m_isFading = true;
            var timer = 0.0f;
            while (timer < m_fadeDuration)
            {
                timer += Time.deltaTime;
                m_canvasGroup.alpha = MathHelpers.Remap(timer,0,m_fadeDuration,0,1);
                yield return null;
            }

            m_canvasGroup.alpha = 1;
            m_isFading = false;
        }
    }
}

