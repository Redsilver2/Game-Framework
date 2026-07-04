using RedSilver2.Framework.Dialogs.Datas;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    public sealed class SubtitleUpdater {
        private List<SubtitleHandler> handlers;

        private float timeElapsed;
        public float TimeElapsed => timeElapsed; 

        public SubtitleUpdater() {
            handlers = new List<SubtitleHandler>();
        }

        public void Play(Transform transform, string characterName, string textToDisplay, float duration)
        {
            if (string.IsNullOrEmpty(textToDisplay)) return;
            GetHandler()?.Play(this, transform, characterName, textToDisplay, duration, 0f);
        }

        public void Play(Transform transform, string characterName, string textToDisplay, float duration, float fadeWaitTime)
        {
            if (string.IsNullOrEmpty(textToDisplay)) return;
            GetHandler()?.Play(this, transform, characterName, textToDisplay, duration, fadeWaitTime);
        }

        public void Play(Dialog dialog) {
            if (dialog == null) return;
            Play(dialog, 0f);
        }

        public void Play(Dialog dialog, float fadeWaitTime)
        {
            if (dialog == null) return;
            Play(dialog.GetSubtitles(), fadeWaitTime);
        }

        private void Play(Subtitle[] subtitles, float fadeWaitTime) {

            if(subtitles == null) return;
            fadeWaitTime = Mathf.Clamp(0f, fadeWaitTime, 1f);

            foreach (Subtitle subtitle in subtitles) {
                if(subtitle == null) continue;
                Play(subtitle, subtitle.GetDatas(), fadeWaitTime);
            }
        }

        private void Play(Subtitle subtitle, SubtitleData[] datas, float fadeWaitTime) {
            if (datas == null || subtitle == null) return;
            foreach (SubtitleData data in datas) GetHandler()?.Play(this, subtitle, data);
        }

        public void Stop() {
            if(handlers == null) return;
            foreach (SubtitleHandler handler in handlers) handler?.Stop(true);

            handlers?.Clear();
        }

        public IEnumerator Update() {
            yield return Update(0f);
        }

        public IEnumerator Update(float startingTime) {
            timeElapsed = Mathf.Clamp(startingTime, 0f, float.MaxValue);

            while (!IsDone()) {
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            Stop();
        }

        private bool IsDone()
        {
            if (handlers == null || handlers.Count <= 0) return true;
            var results01 = handlers.Where(x => x != null);
            var results02 = results01.Where(x => x.IsUpdateFinished);
            return results02.Count() == results01.Count();
        }

        private SubtitleHandler GetHandler()
        {
            DialogManager manager = DialogManager.GetInstance();
            if (manager == null || handlers == null) return null;

            SubtitleHandler handler = manager.GetAvailableSubtitleHandler();
           
            if(!handlers.Contains(handler) && handler != null) 
                handlers?.Add(handler);

            return handler;
        }
    }
}
