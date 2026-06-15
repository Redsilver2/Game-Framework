
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Scenes.UI
{
    public abstract class LoadingScreenUI : MonoBehaviour
    {
        [Space]
        [SerializeField] private float fadeDuration;

        [Space]
        [SerializeField] private float fadeInWaitTime;
        [SerializeField] private float fadeOutWaitTime;

        private IEnumerator fadeOperation;

        protected float FadeInWaitTime => fadeInWaitTime;
        protected float FadeOutWaitTime => fadeOutWaitTime;
        protected float FadeDuration => fadeDuration;


        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (fadeDuration    < 0f) fadeDuration    = 0f;
            if (fadeInWaitTime  < 0f) fadeInWaitTime  = 0f;
            if (fadeOutWaitTime < 0f) fadeOutWaitTime = 0f;
        }
        #endif

        protected virtual void Awake()
        {
            SetAlpha(0f);
        }

        protected virtual void OnDisable() {
            DisableEvent(GameManager.SceneLoaderManager);
        }
        protected virtual void OnEnable() {
            EnableEvent(GameManager.SceneLoaderManager);
        }

        public void StartFade(bool isFadeIn)
        {
            CancelFade();
            fadeOperation = Fade(isFadeIn);
            StartCoroutine(fadeOperation);
        }

        public void CancelFade() {
            if (fadeOperation != null) StopCoroutine(fadeOperation);
            fadeOperation = null;
        }

        protected IEnumerator Fade(bool isFadeIn)
        {
            yield return new WaitForSeconds(isFadeIn ? fadeInWaitTime : fadeOutWaitTime);
            float currentAlpha = GetAlpha();
            float targetAlpha  = isFadeIn ? 1f : 0f;
            float t = 0f;

            while(t < fadeDuration) {
                SetAlpha(Mathf.Lerp(currentAlpha, targetAlpha, t /fadeDuration));
                t += Time.deltaTime;
                yield return null;  
            }

            SetAlpha(targetAlpha);
        }

        public void SetAlpha(bool isVisible) {
            SetAlpha(isVisible ? 1f : 0f);
        }

        public abstract void SetAlpha(float alpha);
        public abstract float GetAlpha();

        public abstract bool IsTargetedAlpha(float alpha);

        protected virtual void EnableEvent(SceneLoaderManager sceneLoader) { }
        protected virtual void DisableEvent(SceneLoaderManager sceneLoader) { }
    }
}
