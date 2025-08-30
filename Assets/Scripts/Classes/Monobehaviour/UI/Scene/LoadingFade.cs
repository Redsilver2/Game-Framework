
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public sealed class LoadingFade : MonoBehaviour
    {
        [Space]
        [SerializeField] private float fadeDuration;

        [Space]
        [SerializeField] private float fadeInWaitTime;
        [SerializeField] private float fadeOutWaitTime;

        private CanvasRenderer renderer;
        private IEnumerator    fadeOperation;

        private bool isFadeCompleted;
        public  bool IsFadeCompleted => isFadeCompleted;


        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (fadeDuration    < 0f) fadeDuration    = 0f;
            if (fadeInWaitTime  < 0f) fadeInWaitTime  = 0f;
            if (fadeOutWaitTime < 0f) fadeOutWaitTime = 0f;
        }
        #endif

        private void Awake()
        {
            renderer        = GetComponent<CanvasRenderer>();
            isFadeCompleted = false;
            SetAlpha(0f);
        }

        public void SetAlpha(bool isVisible)
        {
            if (renderer != null) renderer.SetAlpha(isVisible ? 1f : 0f);
        }

        public void SetAlpha(float alpha)   
        {
            if (renderer != null) renderer.SetAlpha(Mathf.Clamp01(alpha));
        }

        public void StartFade(bool isFadeIn)
        {
            CancelFade();
            fadeOperation = Fade(isFadeIn);
            StartCoroutine(fadeOperation);
        }

        public void CancelFade()
        {
            if (fadeOperation != null) StopCoroutine(fadeOperation);
            fadeOperation = null;
        }

        private IEnumerator Fade(bool isFadeIn)
        {
            if (renderer != null)
            {
                isFadeCompleted = false;
                yield return new WaitForSeconds(isFadeIn ? fadeInWaitTime : fadeOutWaitTime);

                yield return StartCoroutine(renderer.Fade(isFadeIn, fadeDuration));
                isFadeCompleted = true;
            }
        }
    }
}
