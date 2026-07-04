using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private SubtitleHandler template;
        [SerializeField] private DialogChoiceManager choiceManager;

        [Space]
        [SerializeField] private SubtitleDisplayMode subtitleDisplayMode;

        [Space]
        [SerializeField] private float subtitleFadeDuration       = 0.25f;
        [SerializeField] private float subtitleWorldSpaceDistance = 10f;

        [Space]
        [SerializeField] private float subtitleCatchupSpeed;
        [SerializeField][Range(0.01f, 0.25f)] private float subtitleFadeWaitTime;

        [Space]
        [SerializeField] private bool canSubtitleUseWorldSpace;
        [SerializeField] private bool canShowSubtitleByTime;
        [SerializeField] private bool canDisplayCharacterName;

        private UnityEvent<Dialog> onDialogStarted, onDialogFinished;

        private UnityEvent<SubtitleHandler[]> onScreenSpaceSubtitleUpdate;
        private UnityEvent<SubtitleHandler[]> onWorldSpaceSubtitleUpdate;

        private Queue<SubtitleHandler> availabeHandlers;
        private List<SubtitleHandler>  actifSubtitleHandlers;
        private List<SubtitleHandler>  screenSpaceHandlers;

        private Dictionary<Dialog, SubtitleUpdater>      actifDialogs;
        private Dictionary<Dialog, IEnumerator>          dialogUpdaters;
        private Dictionary<SubtitleUpdater, IEnumerator> descriptiveSubtitles;
        
        private readonly static List<SubtitleHandler> subtitleHandlers = new List<SubtitleHandler>();

        public bool CanDisplayCharacterName => canDisplayCharacterName;
        public float SubtitleWorldSpaceDistance => subtitleWorldSpaceDistance;

        public float SubtitleCatchupSpeed => subtitleCatchupSpeed;
        public float SubtitleFadeWaitTime => subtitleFadeWaitTime;

        public bool CanSubtitleUseWorldSpace => canSubtitleUseWorldSpace;
        public bool CanShowSubtitleByTime => canShowSubtitleByTime;

        public float SubtitleFadeDuration => subtitleFadeDuration;
        public SubtitleDisplayMode SubtitleDisplayMode => subtitleDisplayMode;

        private void Awake() {
            actifSubtitleHandlers           = new List<SubtitleHandler>();
            availabeHandlers                = new Queue<SubtitleHandler>();
            
            actifDialogs                    = new Dictionary<Dialog, SubtitleUpdater>();  
            dialogUpdaters                  = new Dictionary<Dialog, IEnumerator>();

            onScreenSpaceSubtitleUpdate     = new UnityEvent<SubtitleHandler[]>();
            onWorldSpaceSubtitleUpdate      = new UnityEvent<SubtitleHandler[]>();

            onDialogStarted      = new UnityEvent<Dialog>();
            onDialogFinished     = new UnityEvent<Dialog>();

            screenSpaceHandlers  = new List<SubtitleHandler>();
            descriptiveSubtitles = new Dictionary<SubtitleUpdater, IEnumerator>();

            StartCoroutine(UpdateSubtitleHandlers());


        }

        private IEnumerator UpdateSubtitleHandlers() {
            while (true) {
                UpdateActifSubtitles();
                yield return null;
            }
        }


        private void UpdateActifSubtitles() {    
            SubtitleHandler[] screenSpaceHandlers;
            Dictionary<Transform, List<SubtitleHandler>> worldSpaceHandlers;

            UpdateScreenSpaceHandlers(out screenSpaceHandlers);
            UpdateWorldSpaceHandlers(out worldSpaceHandlers);

            onScreenSpaceSubtitleUpdate?.Invoke(screenSpaceHandlers.ToArray());

            if(worldSpaceHandlers != null) {
                foreach (List<SubtitleHandler> handlers in worldSpaceHandlers.Values) {
                    if (handlers == null) continue;
                    onWorldSpaceSubtitleUpdate?.Invoke(handlers.ToArray());
                }
            }
        }

        private void UpdateScreenSpaceHandlers(out SubtitleHandler[] results) {

            results = null;
            if (screenSpaceHandlers == null || actifSubtitleHandlers == null) return;

            foreach (SubtitleHandler handler in actifSubtitleHandlers.ToArray()) {
                if (IsInvalidSubtitleHandler(handler)) {
                    if (screenSpaceHandlers.Contains(handler)) screenSpaceHandlers?.Remove(handler);
                    continue;
                }

                UpdateScreenSpaceHandler(handler);
            }

            results = screenSpaceHandlers.Where(x => x != null).ToArray();
        }

        private void UpdateScreenSpaceHandler(SubtitleHandler handler) {
            if (IsInvalidSubtitleHandler(handler) || screenSpaceHandlers == null) return;

            if (handler.IsWorldSpace()) {
                if (!screenSpaceHandlers.Contains(handler)) return;
                screenSpaceHandlers?.Remove(handler);
            }
            else {
                if (screenSpaceHandlers.Contains(handler)) return;
                screenSpaceHandlers?.Add(handler);
            }
        }

        private bool IsInvalidSubtitleHandler(SubtitleHandler handler){
            return handler == null || (!handler.IsUpdateStarted || handler.IsFadedIn());
        }

        private void UpdateWorldSpaceHandlers(out Dictionary<Transform, List<SubtitleHandler>> results)
        {
            results = new Dictionary<Transform, List<SubtitleHandler>>();
            if (actifSubtitleHandlers == null || screenSpaceHandlers == null) return;

            foreach (SubtitleHandler handler in actifSubtitleHandlers.ToArray()) {
                if (IsInvalidSubtitleHandler(handler) || screenSpaceHandlers.Contains(handler)) continue;
                Transform parent = handler.Parent;

                if(parent == null) continue;
                else if (!results.ContainsKey(parent)) results?.Add(parent, new List<SubtitleHandler>());

                results[parent]?.Add(handler);   
            }
        }

        public void Play(DialogInfo info) {
            if (info == null) return;
            Play(info.GetDialog());
        }

        public void Play(string name) {
           Play(Dialog.Get(name)); 
        }

        public void Play(string textToDisplay, float duration, float fadeWaitTime)
        {
            Play(null, string.Empty, textToDisplay, duration, fadeWaitTime, out SubtitleUpdater owner);
        }

        public void Play(string characterName, string textToDisplay, float duration, float fadeWaitTime)
        {
            Play(null, characterName, textToDisplay, duration, fadeWaitTime, out SubtitleUpdater owner);
        }

        public void Play(string textToDisplay, float duration, float fadeWaitTime, out SubtitleUpdater owner)
        {
            Play(null, string.Empty, textToDisplay, duration, fadeWaitTime, out owner);
        }

        public void Play(Transform worldSpaceParent,  string textToDisplay, float duration, float fadeWaitTime)
        {
            Play(worldSpaceParent, string.Empty, textToDisplay, duration, fadeWaitTime, out SubtitleUpdater owner);
        }

        public void Play(Transform worldSpaceParent,  string characterName, string textToDisplay, float duration, float fadeWaitTime)
        {
            Play(worldSpaceParent, characterName, textToDisplay, duration, fadeWaitTime, out SubtitleUpdater owner);
        }

        public void Play(Transform worldSpaceParent,  string textToDisplay, float duration, float fadeWaitTime, out SubtitleUpdater owner) {
            Play(worldSpaceParent, string.Empty, textToDisplay, duration, fadeWaitTime, out owner);
        }

        public void Play(Transform transform, string characterName, string textToDisplay, float duration, float fadeWaitTime, out SubtitleUpdater owner) {
            owner = null;

            if (descriptiveSubtitles == null || string.IsNullOrEmpty(textToDisplay))
                return;

            owner = new SubtitleUpdater();
            owner?.Play(transform, characterName, textToDisplay, duration, fadeWaitTime);
            
            descriptiveSubtitles?.Add(owner, owner.Update());   
            StartCoroutine(descriptiveSubtitles[owner]);
        }

        public void Play(Dialog dialog) 
        {
            if (dialogUpdaters == null || dialog == null || actifDialogs == null || dialogUpdaters.ContainsKey(dialog))
                return;

            StopSimilar(dialog);
            if (dialog.GetChoicesCount() > 0) choiceManager?.Stop();
           
            if (!actifDialogs.ContainsKey(dialog)) actifDialogs?.Add(dialog, new SubtitleUpdater());
            actifDialogs[dialog]?.Play(dialog);

            dialogUpdaters?.Add(dialog, UpdateDialog(dialog));
            StartCoroutine(dialogUpdaters[dialog]);
        }

        public void Stop(DialogInfo info)
        {
            if(info == null) return;
            Stop(info.GetDialog()); 
        }

        public void Stop(Dialog dialog)
        {
            StopDialogUpdater(dialog);
            StopActifDialog(dialog);
        }

        public void Stop() {
           if(dialogUpdaters == null) return;
            choiceManager?.Stop();

            foreach (Dialog key in dialogUpdaters.Keys.ToArray())
                Stop(key);

            StopDescriptiveSubtitles();
            dialogUpdaters?.Clear();   
        }

        private void StopDialogUpdater(Dialog dialog)
        {
            if (dialogUpdaters == null || dialog == null || !dialogUpdaters.ContainsKey(dialog)) return;
            StopCoroutine(dialogUpdaters[dialog]);
            dialogUpdaters?.Remove(dialog);
        }

        private void StopActifDialog(Dialog dialog)
        {
            if (actifDialogs == null || dialog == null || !actifDialogs.ContainsKey(dialog)) return;
            Debug.Log("Stopped: " + dialog);

            actifDialogs[dialog]?.Stop();
            actifDialogs?.Remove(dialog);
        }

        private void StopDescriptiveSubtitles()
        {
            if(descriptiveSubtitles == null) return;
            var updaters = descriptiveSubtitles.Keys;

            foreach (SubtitleUpdater updater in updaters)
               updater?.Stop();

            descriptiveSubtitles?.Clear();
        }

        private void StopDescriptiveSubtitle(SubtitleUpdater updater) { 
            if(descriptiveSubtitles == null || updater == null || !descriptiveSubtitles.ContainsKey(updater)) return;
            StopCoroutine(descriptiveSubtitles[updater]);
            descriptiveSubtitles?.Remove(updater);
            updater?.Stop();
        }

        private void StopSimilar(Dialog current) {
            if (current != null) {
                if (current.GetChoicesCount() > 0) {
                    Stop();
                    return;
                }
            }


            Dialog[] copy = dialogUpdaters.Keys.ToArray();
            if (copy == null) return;

            foreach (Dialog dialog in copy) {
                if (dialog == null) continue;
                else if (dialog.IsSimilar(current) || dialog.Equals(current)) Stop(dialog);
            }
        }

        public void AddOnScreenSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if (action != null) onScreenSpaceSubtitleUpdate?.AddListener(action);
        }
        public void RemoveOnScreenSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if(action != null) onScreenSpaceSubtitleUpdate?.RemoveListener(action);
        }

        public void AddOnWorldSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if (action != null) onWorldSpaceSubtitleUpdate?.AddListener(action);
        }

        public void RemoveOnWorldSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if (action != null) onWorldSpaceSubtitleUpdate?.RemoveListener(action);
        }

        public void AddOnDialogFinishedListener(UnityAction<Dialog> action) {
            if(action != null) onDialogFinished?.AddListener(action);
        }
        public void RemoveOnDialogFinishedListener(UnityAction<Dialog> action) {
            if (action != null) onDialogFinished?.RemoveListener(action);
        }

        public void AddOnDialogStartedListener(UnityAction<Dialog> action)
        {
            if (action != null) onDialogStarted?.AddListener(action);
        }
        public void RemoveOnDialogStartedListener(UnityAction<Dialog> action)
        {
            if (action != null) onDialogStarted?.RemoveListener(action);
        }

        public void AddActifSubtitleHandler(SubtitleHandler handler)
        {
            if (!IsValidHandler(handler) || actifSubtitleHandlers == null  || actifSubtitleHandlers.Contains(handler))  
                return;

            actifSubtitleHandlers?.Add(handler);
        }

        private void RemoveActifSubtitleHandler(SubtitleHandler handler)
        {
            if (!IsValidHandler(handler) || actifSubtitleHandlers == null || !actifSubtitleHandlers.Contains(handler))
                return;

            actifSubtitleHandlers?.Remove(handler);
        }

        public void AddAvailableSubtitleHandler(SubtitleHandler handler)
        {
            if(!IsValidHandler(handler) || availabeHandlers == null || availabeHandlers.Contains(handler)) return;
            RemoveActifSubtitleHandler(handler);

            availabeHandlers?.Enqueue(handler);
        }


        private bool IsValidHandler(SubtitleHandler handler)
        {
            if (subtitleHandlers == null || handler == null) return false;
            return subtitleHandlers.Contains(handler);
        }


        private IEnumerator UpdateDialog(Dialog dialog) {
            if (actifDialogs != null && dialog != null) {
                if(actifDialogs.ContainsKey(dialog)) {
                    SubtitleUpdater updater = actifDialogs[dialog];
                    if (updater != null) yield return StartCoroutine(updater.Update());
                    if (choiceManager != null && dialog.GetChoicesCount() > 0) yield return StartCoroutine(choiceManager.UpdateChoices(dialog));
                }
            }

            Stop(dialog);
            yield return null;
        }

        private void AddHandlerDisplayer(SubtitleHandler handler, ref List<SubtitleHandler> handlers)
        {
            if (handlers == null || handler == null || handlers.Contains(handler)) return;
            handlers?.Add(handler);
        }

        private void RemoveHandlerDisplayer(SubtitleHandler handler, ref List<SubtitleHandler> handlers)
        {
            if (handlers == null || handler == null || !handlers.Contains(handler)) return;
            handlers?.Remove(handler);
        }

        public SubtitleHandler GetAvailableSubtitleHandler()
        {
            SubtitleHandler current = null;
           
            if (template != null) {
                if (availabeHandlers != null) {
                    if (availabeHandlers.Count > 0) current = availabeHandlers.Dequeue();
                    else current = Instantiate(template);
                }
                else current = Instantiate(template);
            }

            current?.Stop(false);
            AddActifSubtitleHandler(current);

            return current;
        }

        public static void AddSubtitleHandlerInstance(SubtitleHandler handler)
        {
            if (handler == null || subtitleHandlers == null) return;
            if (!subtitleHandlers.Contains(handler)) subtitleHandlers?.Add(handler);
        }


        public static void RemoveSubtitleHandlerInstance(SubtitleHandler handler)
        {
            if (handler == null || subtitleHandlers == null) return;
            else if (subtitleHandlers.Contains(handler)) subtitleHandlers?.Remove(handler);
        }

        public static DialogManager GetInstance() {
            return GameManager.DialogManager;
        }

        public static DialogChoiceManager GetChoiceManager() {
            DialogManager manager = GetInstance();
            return manager != null ? manager.choiceManager : null;
        }
    }
}