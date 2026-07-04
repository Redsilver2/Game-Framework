using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Player;
using RedSilver2.Framework.StateMachines.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Dialogs
{
    public class DialogChoiceManager : MonoBehaviour {
        [SerializeField] private DialogChoiceHandler template;
        [SerializeField] private Transform parent;

        [Space]
        [SerializeField] private bool canMakeRandomChoices;
        [SerializeField] private bool useMouseForChoices;

        private bool isMakingChoices;
        private bool isChoiceMade;

        private Queue<DialogChoiceHandler> availableChoiceHandlers;
        private List<DialogChoiceHandler>  actifChoiceHandlers;

        private UnityEvent<DialogChoiceHandler[]> onDialogChoiceUpdated;
        private UnityEvent<float, float> onChoiceTimerProgressionChanged;

        private UnityEvent<DialogChoiceHandler> onChoiceSelected, onChoiceDeselected;
        private UnityEvent<DialogChoice[]> onChoiceUpdateStarted, onChoiceUpdateFinished;

        private static readonly List<DialogChoiceHandler> choiceHandlers = new List<DialogChoiceHandler>();

        private void Awake()
        {
            onChoiceDeselected = new UnityEvent<DialogChoiceHandler>();
            onChoiceSelected   = new UnityEvent<DialogChoiceHandler>();

            onChoiceTimerProgressionChanged = new UnityEvent<float, float>();

            onChoiceUpdateStarted = new UnityEvent<DialogChoice[]>();
            onChoiceUpdateFinished   = new UnityEvent<DialogChoice[]>();

            availableChoiceHandlers = new Queue<DialogChoiceHandler>();
            actifChoiceHandlers = new List<DialogChoiceHandler>();

            onDialogChoiceUpdated = new UnityEvent<DialogChoiceHandler[]>();
            
            isMakingChoices = false;
            isChoiceMade = false;

            AddOnChoiceUpdateStartedListener(OnChoiceUpdateStarted);
            AddOnChoiceUpdateFinishedListener(OnChoiceUpdateFinished);
            AddOnHandlersUpdateListener(OnHandlersUpdate);

            if (template != null) template.gameObject.SetActive(false);
        }

        public void AddOnHandlersUpdateListener(UnityAction<DialogChoiceHandler[]> action)
        {
            if (action != null) onDialogChoiceUpdated?.AddListener(action);
        }
        public void RemoveOnHandlersUpdateListener(UnityAction<DialogChoiceHandler[]> action)
        {
            if (action != null) onDialogChoiceUpdated?.RemoveListener(action);
        }

        public void AddOnChoiceUpdateStartedListener(UnityAction<DialogChoice[]> action)
        {
            if (action != null) onChoiceUpdateStarted?.AddListener(action);
        }
        public void RemoveOnChoiceUpdateStartedListener(UnityAction<DialogChoice[]> action)
        {
            if (action != null) onChoiceUpdateStarted?.RemoveListener(action);
        }

        public void AddOnChoiceUpdateFinishedListener(UnityAction<DialogChoice[]> action)
        {
            if (action != null) onChoiceUpdateFinished?.AddListener(action);
        }
        public void RemoveOnChoiceUpdateFinishedListener(UnityAction<DialogChoice[]> action)
        {
            if (action != null) onChoiceUpdateFinished?.RemoveListener(action);
        }

        public void AddOnChoiceDeselectedListener(UnityAction<DialogChoiceHandler> action)
        {
            if (action != null) onChoiceDeselected?.AddListener(action);
        }
        public void RemoveOnChoiceDeselectedListener(UnityAction<DialogChoiceHandler> action)
        {
            if (action != null) onChoiceDeselected?.RemoveListener(action);
        }

        public void AddOnChoiceSelectedListener(UnityAction<DialogChoiceHandler> action)
        {
            if (action != null) onChoiceSelected?.AddListener(action);
        }
        public void RemoveOnChoiceSelectedListener(UnityAction<DialogChoiceHandler> action)
        {
            if (action != null) onChoiceSelected?.RemoveListener(action);
        }

        private void OnHandlersUpdate(DialogChoiceHandler[] handlers)
        {
            if (handlers == null) return;

            foreach (DialogChoiceHandler handler in handlers) {
                if(handler ==  null) continue;
                handler.transform.SetParent(parent);
            }
        }

        private void OnChoiceUpdateStarted(DialogChoice[] choices) {
            if (choices == null || actifChoiceHandlers == null) return;

            for (int i = 0; i < choices.Length; i++) {
                DialogChoiceHandler handler = GetChoiceHandler();
                DialogChoice choice = choices[i];

                if (handler == null) break;
                else if (choice == null) continue;

                handler.gameObject.SetActive(true);
                handler?.SetChoice(choice);

                actifChoiceHandlers?.Add(handler);
            }

            isChoiceMade = false;
            isMakingChoices = true;
        }


        private void OnChoiceUpdateFinished(DialogChoice[] choices) {
            if (actifChoiceHandlers == null) return;

            foreach(DialogChoiceHandler handler in actifChoiceHandlers.ToArray()) {
                if (handler == null) continue;
                handler.gameObject.SetActive(false);
                actifChoiceHandlers?.Remove(handler);
                availableChoiceHandlers?.Enqueue(handler);
            }

            isChoiceMade    = false;
            isMakingChoices = false;
        }

        public void Stop() {
            isMakingChoices = false;
            isChoiceMade    = false;
        }

        public IEnumerator UpdateChoices(Dialog dialog)
        {
            float t = dialog != null ? dialog.ChoiceDuration : 1f;
            int choiceIndex = 0, previousChoiceIndex = -1;

            DialogChoice[] choices = dialog != null ? dialog.GetChoices().Where(x => x != null).ToArray()
                                                    : null;

            onChoiceUpdateStarted?.Invoke(choices);

            while (dialog != null && choices != null && !isChoiceMade) {
                if (actifChoiceHandlers == null || actifChoiceHandlers.Count == 0 || !isMakingChoices) break;
                onDialogChoiceUpdated?.Invoke(actifChoiceHandlers.ToArray());

                UpdateChoiceTimer(dialog, ref t);
                UpdateChoiceIndex(choices, ref choiceIndex, ref previousChoiceIndex);
                MakeChoice(choices, t, ref choiceIndex);
                yield return null;
            }

            onChoiceUpdateFinished?.Invoke(choices);
        }

        private void UpdateChoiceTimer(Dialog dialog, ref float time)
        {
            if (dialog == null) return;

            bool canUpdateChoiceDuration = !dialog.CanUpdateChoiceDuration;
            float duration = dialog.ChoiceDuration;

            if (!dialog.CanUpdateChoiceDuration)
                time = Mathf.Clamp(time - Time.deltaTime, 0f, duration);
            else
                time = duration;

            onChoiceTimerProgressionChanged?.Invoke(Mathf.Clamp(time, 0f, float.MaxValue), Mathf.Clamp01(time / duration));
        }

        private void UpdateChoiceIndex(DialogChoice[] choices, ref int currentIndex, ref int previousIndex)
        {
            if (choices == null || choices.Length == 0) return;
            else if (InputManager.GetKeyDown(KeyboardKey.DownArrow)) currentIndex--;
            else if (InputManager.GetKeyDown(KeyboardKey.UpArrow)) currentIndex++;

            currentIndex = Mathf.Clamp(currentIndex, 0, choices.Length - 1);

            if (previousIndex != currentIndex)
            {
                if(previousIndex > 0) onChoiceDeselected?.Invoke(actifChoiceHandlers[previousIndex]);
                onChoiceSelected?.Invoke(actifChoiceHandlers[currentIndex]);
                previousIndex = currentIndex;
            }
        }

        public void MakeChoice()
        {
            isChoiceMade = true;
        }

        private void MakeChoice(DialogChoice[] choices, float time, ref int choiceIndex)
        {
            if (actifChoiceHandlers == null || actifChoiceHandlers.Count == 0 || choiceIndex < 0 || choiceIndex >= actifChoiceHandlers.Count)
                return;
            else if (choices == null || choices.Length == 0) return;

            DialogChoiceHandler handler = actifChoiceHandlers[choiceIndex];
            if (handler == null) return;

            if (time < 0f) {
                if (canMakeRandomChoices) choiceIndex = Random.Range(0, choices.Length);
                isChoiceMade = true;
            }
            else if (InputManager.GetKeyDown(KeyboardKey.C)) 
                isChoiceMade = true;

            if (isChoiceMade) { choices[choiceIndex]?.Choose(); }
        }

        private DialogChoiceHandler GetChoiceHandler()
        {
            if(availableChoiceHandlers != null) {
                if (availableChoiceHandlers.Count > 0)
                    return availableChoiceHandlers.Dequeue();
            }

            return template != null ? Instantiate(template) : null;
        }

        public static void AddChoiceHandlerInstance(DialogChoiceHandler handler) {
            if (handler == null || choiceHandlers == null || choiceHandlers.Contains(handler)) return;
            choiceHandlers?.Add(handler);
        }

        public static void RemoveChoiceHandlerInstance(DialogChoiceHandler handler)
        {
            if (handler == null || choiceHandlers == null || !choiceHandlers.Contains(handler)) return;
            choiceHandlers?.Remove(handler);
        }

        public static DialogChoiceManager GetInstance() { return DialogManager.GetChoiceManager(); }
    }
}
