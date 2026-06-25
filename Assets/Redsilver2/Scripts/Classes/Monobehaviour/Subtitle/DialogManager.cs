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
        [SerializeField] private float subtitleFadeDuration       = 0.25f;
        [SerializeField] private float subtitleWorldSpaceDistance = 10f;

        [Space]
        [SerializeField] private float subtitleCatchupSpeed;
        [SerializeField][Range(0.01f, 0.25f)] private float subtitleFadeSpeed;

        [Space]
        [SerializeField] private bool canSubtitleUseWorldSpace;
        [SerializeField] private bool canShowSubtitleByTime;
        [SerializeField] private bool canDisplayCharacterName;

        private UnityEvent<Dialog> onDialogStarted, onDialogFinished;

        private UnityEvent<SubtitleHandler[]> onScreenSpaceSubtitleUpdate;
        private UnityEvent<SubtitleHandler[]> onWorldSpaceSubtitleUpdate;

        private Queue<SubtitleHandler> availabeHandlers;
        private List<SubtitleHandler>  actifSubtitleHandlers;

        private Dictionary<Dialog, ActifSubtitleUpdater> actifDialogs;
        private Dictionary<Dialog, IEnumerator>     dialogUpdaters;
        
        private readonly static List<SubtitleHandler> subtitleHandlers = new List<SubtitleHandler>();

        public bool CanDisplayCharacterName => canDisplayCharacterName;
        public float SubtitleWorldSpaceDistance => subtitleWorldSpaceDistance;

        public float SubtitleCatchupSpeed => subtitleCatchupSpeed;
        public float SubtitleFadeSpeed => subtitleFadeSpeed;

        public bool CanSubtitleUseWorldSpace => canSubtitleUseWorldSpace;
        public bool CanShowSubtitleByTime => canShowSubtitleByTime;

        public float SubtitleFadeDuration => subtitleFadeDuration;

        private void Awake() {
            actifSubtitleHandlers           = new List<SubtitleHandler>();
            availabeHandlers                = new Queue<SubtitleHandler>();
            
            actifDialogs                    = new Dictionary<Dialog, ActifSubtitleUpdater>();  
            dialogUpdaters                  = new Dictionary<Dialog, IEnumerator>();

            InitializeSubtitleUpdateEvents();
            InitializeDialogEvents();
        }

        private void Update() {
            if (subtitleHandlers != null) { 
                onScreenSpaceSubtitleUpdate?.Invoke(actifSubtitleHandlers.ToArray());
                //onWorldSpaceSubtitleUpdate?.Invoke(null);
            }
        }

        private void InitializeSubtitleUpdateEvents() {
            onScreenSpaceSubtitleUpdate = new UnityEvent<SubtitleHandler[]>();
            onWorldSpaceSubtitleUpdate  = new UnityEvent<SubtitleHandler[]>();
        }

        private void InitializeDialogEvents()
        {
            onDialogStarted = new UnityEvent<Dialog>();
            onDialogFinished = new UnityEvent<Dialog>();
        }

        public void Play(DialogInfo info) {
            if (info == null) return;
            Play(info.GetDialog());
        }

        public void Play(string name) {
           Play(Dialog.Get(name)); 
        }

        public void Play(Dialog dialog) 
        {
            if (dialogUpdaters == null || dialog == null || actifDialogs == null || dialogUpdaters.ContainsKey(dialog))
                return;

            StopSimilar(dialog);
            if (dialog.GetChoicesCount() > 0) choiceManager?.Stop();
           
            if (!actifDialogs.ContainsKey(dialog)) actifDialogs?.Add(dialog, new ActifSubtitleUpdater(dialog));
            actifDialogs[dialog]?.Play();

            dialogUpdaters?.Add(dialog, DialogUpdate(dialog));
            StartCoroutine(dialogUpdaters[dialog]);
        }

        public void Stop(Dialog dialog)
        {
            if (dialogUpdaters == null || actifDialogs == null)
                return;

            if (dialogUpdaters.ContainsKey(dialog)) {
                StopCoroutine(dialogUpdaters[dialog]);
                dialogUpdaters?.Remove(dialog);
            }

            if (actifDialogs.ContainsKey(dialog)) {
                actifDialogs[dialog]?.Stop();
                actifDialogs?.Remove(dialog);
            }
        }

        public void Stop() {
           if(dialogUpdaters == null) return;
            choiceManager?.Stop();

            foreach (Dialog key in dialogUpdaters.Keys.ToArray())
                Stop(key);
           
            dialogUpdaters?.Clear();   
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
                else if (dialog.IsSimilar(current)) Stop(current);
            }
        }

        public void AddOnScreenSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if (action != null) onScreenSpaceSubtitleUpdate?.AddListener(action);
        }
        public void RemoveOnScreenSpaceSubtitleUpdateListener(UnityAction<SubtitleHandler[]> action) {
            if(action != null) onScreenSpaceSubtitleUpdate?.RemoveListener(action);
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
            if (subtitleHandlers == null || !subtitleHandlers.Contains(handler)) return;
            else if (actifSubtitleHandlers == null || handler == null || actifSubtitleHandlers.Contains(handler))
                return;

            actifSubtitleHandlers?.Add(handler);
        }

        public void RemoveActifSubtitleHandler(SubtitleHandler handler)
        {
            if (subtitleHandlers == null || !subtitleHandlers.Contains(handler)) return;
            else if(actifSubtitleHandlers == null || handler == null || !actifSubtitleHandlers.Contains(handler))
                return;

            actifSubtitleHandlers?.Remove(handler);
        }


        public void AddAvailableSubtitleHandler(SubtitleHandler handler)
        {
            if(subtitleHandlers == null || !subtitleHandlers.Contains(handler)) return;
            else if (handler == null || availabeHandlers == null || availabeHandlers.Contains(handler)) return;

            availabeHandlers?.Enqueue(handler);
        }

        private IEnumerator DialogUpdate(Dialog dialog)
        {
            float t = 0f;

            while (dialog != null)
            {
                if (actifDialogs == null || !actifDialogs.ContainsKey(dialog)) break;

                ActifSubtitleUpdater subtitleUpdater = actifDialogs[dialog];
                subtitleUpdater?.Update(this, t);

                if (subtitleUpdater.IsDone()) {
                    if (dialog.GetChoicesCount() > 0 && choiceManager != null)
                        yield return StartCoroutine(choiceManager.UpdateChoices(dialog));
                    break;
                }

                t += Time.deltaTime;
                yield return null;
            }

            Stop(dialog);
            yield return null;
        }

        public SubtitleHandler GetAvailableSubtitleHandler()
        {
            SubtitleHandler current;
            if (template == null) return null;

            if (availabeHandlers != null) {
                if (availabeHandlers.Count > 0) { current = availabeHandlers.Dequeue(); }
                else { current = Instantiate(template); }
            }
            else { current = Instantiate(template); }
          
            actifSubtitleHandlers?.Add(current);
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