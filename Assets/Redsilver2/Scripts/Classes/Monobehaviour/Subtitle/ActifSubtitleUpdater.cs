using RedSilver2.Framework.Dialogs.Datas;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    public class ActifSubtitleUpdater {
        private bool isInitialized;
        private bool isPlaying;

        private readonly Dialog dialog;
        private readonly Dictionary<Subtitle, List<SubtitleHandler>> subtitleHandlers;

        public ActifSubtitleUpdater(Dialog dialog) {
            this.dialog        = dialog;
            this.isInitialized = false;
            this.isPlaying     = false;

            subtitleHandlers   = new Dictionary<Subtitle, List<SubtitleHandler>>();
            Initialize();
        }

        public void Play()
        {
            if (!isInitialized || isPlaying || dialog == null) return;
            
            foreach(Subtitle subtitle in dialog.GetSubtitles()) {
                if(subtitle == null) continue;
                subtitle?.Play();
            }

            isPlaying = true;
        }

        public void Stop() {
            if (!isInitialized || !isPlaying || dialog == null) return;
            
            foreach (Subtitle subtitle in dialog.GetSubtitles()) {
                if (subtitle == null) continue;
                Stop(subtitle);
            }

            isPlaying = false;
        }

        private void Stop(Subtitle subtitle)
        {
            subtitle?.Stop();

            if (subtitleHandlers == null || !subtitleHandlers.ContainsKey(subtitle))
                return;

            foreach (SubtitleHandler handler in subtitleHandlers[subtitle]) {
                GameManager.DialogManager.AddAvailableSubtitleHandler(handler);
                handler?.Stop();
            }
        }


        public void Update(DialogManager manager, float timeElapsed) {
            if (dialog == null || manager == null) return; 
            Update(manager, dialog.GetSubtitles(), timeElapsed);
        }

        private void Update(DialogManager manager,  Subtitle[] subtitles, float timeElapsed) {
            if (subtitles == null || subtitleHandlers == null) return;

            foreach (Subtitle subtitle in subtitles)
                Update(manager, subtitle, timeElapsed);
        }

        private void Update(DialogManager manager, Subtitle subtitle, float timeElapsed)
        {
            if (subtitle == null || subtitleHandlers == null || !subtitleHandlers.ContainsKey(subtitle))
                return;
           
            Update(manager, subtitle, subtitle.GetDatas(), timeElapsed);
        }

        private void Update(DialogManager manager, Subtitle subtitle, SubtitleData[] datas, float timeElapsed) {
            if (subtitle == null || datas == null || subtitleHandlers == null || !subtitleHandlers.ContainsKey(subtitle))
                return;

            for(int i = 0; i < datas.Length; i++) {
                SubtitleHandler[] handlers = subtitleHandlers[subtitle].ToArray();
               
                if (handlers == null || handlers.Length <= 0 || i < 0 || i >= handlers.Length) break;
                handlers[i]?.UpdateDisplayers(manager, subtitle, i, timeElapsed);
            }
        }

        public void Initialize() {
            if (dialog == null || subtitleHandlers == null || isInitialized) return;

            foreach (Subtitle subtitle in dialog.GetSubtitles()) {
                if (subtitle == null) continue;

                if (!subtitleHandlers.ContainsKey(subtitle))  subtitleHandlers?.Add(subtitle, new List<SubtitleHandler>());
                Initialize(subtitle);
            }

            isInitialized = true;
        }

        private void Initialize(Subtitle subtitle)
        {
            DialogManager manager = GameManager.DialogManager;

            if (manager == null) return;
            else if (subtitleHandlers == null || subtitle == null || isInitialized || !subtitleHandlers.ContainsKey(subtitle))
                return;

            foreach (SubtitleData data in subtitle.GetDatas()) {
                SubtitleHandler handler = manager.GetAvailableSubtitleHandler();
                if (subtitleHandlers[subtitle].Contains(handler)) continue;
                subtitleHandlers[subtitle]?.Add(handler);
            }
        }

        public bool IsDone()
        {
            if (dialog == null) return true;
            return IsDone(dialog.GetSubtitles());
        }

        private bool IsDone(Subtitle[] subtitles) {
            if (subtitles == null || !isInitialized) return true;
            var results = subtitles.Where(x => x != null);
            return results.Count() == results.Where(x => IsDone(x)).Count();
        }

        private bool IsDone(Subtitle subtitle) {
            if (subtitle == null || subtitleHandlers == null || !subtitleHandlers.ContainsKey(subtitle) || subtitleHandlers[subtitle] == null)
                return true;

            SubtitleHandler[] handlers = subtitleHandlers[subtitle].Where(x => x != null).ToArray();
            return handlers.Length == handlers.Where(x => x.IsUpdateFinished).Count();
        }
    }
}
