using System.Collections;
using System.Threading;
using UnityEngine;


namespace RedSilver2.Framework.Subtitles
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Subtitle : MonoBehaviour {

        private CanvasGroup canvasGroup;
        private CancellationTokenSource subtitleUpdateTokenSource, subtitleFadeTokenSource;
        private SubtitleHandler handler;

        protected virtual void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
        }

        public void SetSubtitleHandler(SubtitleHandler handler){
            this.handler = handler;
        }

        public void Play(string characterName, string contextText, float duration, float fadeDelay) {
            StartCoroutine(UpdateSubtitle(characterName, contextText, duration, fadeDelay, GetCancellationToken(ref subtitleUpdateTokenSource)));
        }

        public void PlayDescription(string characterName, string contextText, float duration, float fadeDelay) {
            StartCoroutine(UpdateDescription(characterName, contextText, duration, fadeDelay, GetCancellationToken(ref subtitleUpdateTokenSource)));
        }


        public void Stop()
        {
            if (subtitleUpdateTokenSource != null)
            {
                subtitleUpdateTokenSource.Cancel();
                subtitleUpdateTokenSource = null;
            }

            if(subtitleFadeTokenSource != null) {
                subtitleFadeTokenSource.Cancel();
                subtitleFadeTokenSource = null;
            }

            SubtitleManager subtitleManager = SubtitleManager.Instance;

            if (subtitleManager != null) {
                subtitleManager.RemoveScreenSubtitle(this);

                if (handler != null)
                {
                    handler.RemoveActifSubtitle(this);
                    handler = null;
                }
            }

            if (canvasGroup != null) canvasGroup.alpha = 0f;
            UpdateDisplayers(string.Empty, string.Empty);
        }


        public void Skip() {
            StartCoroutine(EndSubtitle(" ", " ", 0.5f, GetCancellationToken(ref subtitleUpdateTokenSource)));
        }

        protected IEnumerator UpdateSubtitle(string characterName, string context, float duration, float fadeDelay, CancellationToken token)
        {
            float  timeElapsed = 0f;
            SubtitleManager subtitleManager = SubtitleManager.Instance;

            if (subtitleManager != null)
                StartCoroutine(Fade(true, 0.1f, 0f));

            if (!string.IsNullOrEmpty(context)) {
                while (timeElapsed < duration && !token.IsCancellationRequested)  {
                    string characterNameResult = GetText(string.IsNullOrEmpty(characterName) ? string.Empty : characterName, Mathf.Clamp01(timeElapsed / 0.5f), SubtitleDisplayMode.Instant);
                    string contextResult       = GetText(context, Mathf.Clamp01(timeElapsed / duration), SubtitleDisplayMode.Progressif);

                    UpdateDisplayers(characterNameResult, contextResult);
                    timeElapsed = Mathf.Clamp(Time.deltaTime + timeElapsed, 0f, duration);
                    yield return null;
                }

                if (timeElapsed >= duration) 
                    yield return StartCoroutine(EndSubtitle(string.IsNullOrEmpty(characterName) ? string.Empty : characterName, context, fadeDelay, token));


                yield return null;
            }
        }

        private IEnumerator UpdateDescription(string characterName, string context, float duration, float fadeDelay, CancellationToken token) {
            float t = 0f;
            SubtitleManager subtitleManager = SubtitleManager.Instance;

            if (subtitleManager != null)
                StartCoroutine(Fade(true, 0.1f, 0f));


            UpdateDisplayers(string.IsNullOrEmpty(characterName) ? string.Empty : characterName, $"[{context}]");

            while(t < duration && !token.IsCancellationRequested) {
                t += Time.deltaTime;
                yield return null;
            }

            if(t >= duration)
               yield return StartCoroutine(EndSubtitle(string.IsNullOrEmpty(characterName) ? string.Empty : characterName, $"[{context}]", fadeDelay, token));
        }

        private IEnumerator EndSubtitle(string characterName, string contextText, float fadeDelay, CancellationToken token)
        {
            SubtitleManager subtitleManager = SubtitleManager.Instance;
            UpdateDisplayers($"{characterName}:", contextText);

            if (!token.IsCancellationRequested) {
                if (subtitleManager != null)
                {
                    yield return StartCoroutine(Fade(false, subtitleManager.SubtitleFadeDuration, fadeDelay));
                    subtitleManager.RemoveScreenSubtitle(this);

                    if(handler != null){
                        handler.RemoveActifSubtitle(this);
                        handler = null;
                    }             
                }

                
            }
        }

        private IEnumerator Fade(bool isVisible, float duration, float delay) {
            float currentAlpha = canvasGroup.alpha;
            float t = 0f;

            CancellationToken token = GetCancellationToken(ref subtitleFadeTokenSource);

            while(t < delay && !token.IsCancellationRequested) {
                t += Time.deltaTime;
                yield return null;
            }

            t = 0f;

            while (!token.IsCancellationRequested) {
                if (t >= duration) {
                    canvasGroup.alpha = isVisible ? 1f : 0f;
                    break;
                }

                canvasGroup.alpha = Mathf.Lerp(currentAlpha, isVisible ? 1f : 0f, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
        }

        protected string GetText(string text, float progress, SubtitleDisplayMode displayMode)
        {
            if      (string.IsNullOrEmpty(text))                                   return string.Empty;
            else if (displayMode == SubtitleDisplayMode.Instant || progress >= 1f) return text;
            return GetText(text, progress);
        }

        protected virtual string GetText(string text, float progress) {            

            if (string.IsNullOrEmpty(text)) return string.Empty;
            else if(progress >= 1f)         return text;

            char[] characters = text.ToCharArray();
            int    maxIndex   = (int)(characters.Length * Mathf.Clamp01(progress));

            string resultat = string.Empty;
            for (int i = 0; i <= maxIndex; i++)  resultat += characters[i];

            return resultat;
        }

        private CancellationToken GetCancellationToken(ref CancellationTokenSource tokenSource) {
            if (tokenSource != null) tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();
            return tokenSource.Token;
        }

        public abstract int GetLineCount();
        protected abstract void UpdateDisplayers(string characterName, string context);
    }
}
