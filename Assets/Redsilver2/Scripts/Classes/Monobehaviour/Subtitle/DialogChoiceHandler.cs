using RedSilver2.Framework.Inputs;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RedSilver2.Framework.Dialogs
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DialogChoiceHandler : MonoBehaviour {   
        [SerializeField] private TextMeshProUGUI textDisplayer;
        [SerializeField] private Button button;
   
        private DialogChoice choice;

        private UnityEvent<DialogChoice> onChoiceChanged;
        private UnityEvent onSelected, onDeselected;

        public TextMeshProUGUI TextDisplayer   => textDisplayer;

        private void Awake() {
            onChoiceChanged = new UnityEvent<DialogChoice>();   
            AddOnChoiceChangedListener(OnChoiceChanged);

            if (button != null) button.onClick.AddListener(() => {
                DialogChoiceManager.GetInstance()?.MakeChoice();
                choice?.Choose();
            });

            onChoiceChanged?.Invoke(null);

            AddOnChoiceChangedListener(OnChoiceChanged);
            SetDefaultEvents(DialogChoiceManager.GetInstance(), true);
        }

        private void OnEnable() {
            SetDefaultEvents(DialogChoiceManager.GetInstance(), true);
            DialogChoiceManager.AddChoiceHandlerInstance(this);  
        }

        private void OnDisable()
        {
           SetDefaultEvents(DialogChoiceManager.GetInstance(), false);
           DialogChoiceManager.RemoveChoiceHandlerInstance(this);
        }

        protected virtual void SetDefaultEvents(DialogChoiceManager manager, bool isAddingEvents)
        {
            if (isAddingEvents) {
                manager?.AddOnChoiceSelectedListener(OnSelected);
                manager?.AddOnChoiceDeselectedListener(OnDeselected);
            }
            else {
                manager?.RemoveOnChoiceSelectedListener(OnSelected);
                manager?.RemoveOnChoiceDeselectedListener(OnDeselected);
            }
        }

        public void SetChoice(DialogChoice choice) {
            if (this.choice != choice) onChoiceChanged?.Invoke(choice);
        }

        public void AddOnChoiceChangedListener(UnityAction<DialogChoice> action)
        {
            if (action != null) onChoiceChanged?.AddListener(action);
        }
        public void RemoveOnChoiceChangedListener(UnityAction<DialogChoice> action)
        {
            if (action != null) onChoiceChanged?.RemoveListener(action);
        }

        public void AddOnSelectedListener(UnityAction action) {
            if (action != null) onSelected?.AddListener(action);
        }
        public void RemoveOnSelectedListener(UnityAction action) {
            if (action != null) onSelected?.RemoveListener(action);
        }

        public void AddOnDeselectedListener(UnityAction action) {
            if (action != null) onDeselected?.AddListener(action);
        }
        public void RemoveOnDeselectedListener(UnityAction action)
        {
            if (action != null) onDeselected?.RemoveListener(action);
        }


        protected virtual void OnChoiceChanged(DialogChoice choice) {
            this.choice = choice;

            SetTextDisplayerState(choice == null ? false       : true);
            SetTextDisplayerText (choice  == null ? string.Empty : choice.Description);
        }

        private void SetTextDisplayerState(bool enable) {
            if(textDisplayer != null) textDisplayer.gameObject.SetActive(enable);
        }
        private void SetTextDisplayerText(string text) {
            if (textDisplayer != null) textDisplayer.text = text;
        }

        private void OnSelected(DialogChoiceHandler handler) {
            if (handler != this) return;
            onSelected?.Invoke();
        }
        private void OnDeselected(DialogChoiceHandler handler)
        {
            if (handler != this) return;
            onDeselected?.Invoke();
        }
    }
}
