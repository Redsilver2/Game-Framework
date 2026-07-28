using RedSilver2.Framework.Interactions.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactableName;
        [SerializeField] private Transform uiParent;

        [Space]
        [SerializeField] private bool isInteractable;

        private int currentSelectedIndex = 0, previousSelectedIndex = -1;

        private Collider _collider;
        private IEnumerator selectionUpdateCoroutine;

        private List<InteractionAction> actions;

        private UnityEvent<int> onSelectionIndexChanged;
        private UnityEvent<InteractionAction> onInteractionActionAdded, onInteractionActionRemoved;
        private UnityEvent<InteractionHandler> onSelected, onUnselected;

        public string InteractableName => interactableName;
        public bool   IsInteractable   => isInteractable;

        public InteractionType Type { get; private set; }

        public Transform UIParent => uiParent;
        public Collider Collider => _collider;


        protected virtual void Awake() {
            _collider                 = GetComponent<Collider>();
            actions                   = new List<InteractionAction>();

            onSelected                = new UnityEvent<InteractionHandler>();
            onUnselected              = new UnityEvent<InteractionHandler>();

            onInteractionActionAdded   = new UnityEvent<InteractionAction>();
            onInteractionActionRemoved = new UnityEvent<InteractionAction>();

            AddOnSelectedListener(StartSelectionUpdate);
            AddOnUnselectedListener(StopSelectionUpdate);

            gameObject.layer = GameManager.InteractionLayer;
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        protected virtual void OnEnable() {
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        protected virtual void OnDisable() {
            InteractionHandler.RemoveInteractionModuleInstance(_collider);
        }

        public virtual void SetIsInteractable(bool isInteractable)
        {
            this.isInteractable = isInteractable;
        }

        private void StartSelectionUpdate(InteractionHandler handler) {
            StopSelectionUpdate(handler);
            selectionUpdateCoroutine = SelectionUpdate(handler);
            StartCoroutine(selectionUpdateCoroutine);
        }

        private void StopSelectionUpdate(InteractionHandler handler) {
            if (selectionUpdateCoroutine != null) StopCoroutine(selectionUpdateCoroutine);
            selectionUpdateCoroutine      = null;
        }

        private IEnumerator SelectionUpdate(InteractionHandler handler) {
            onSelectionIndexChanged?.Invoke(currentSelectedIndex);

            while (handler != null && actions != null) {
                if (this.actions.Count <= 0) {
                    yield return null;
                    continue;
                }

                UpdateSelectionIndex(handler, this.actions.ToArray());
                this.actions[currentSelectedIndex]?.UpdateAction(handler);
                yield return null;
            }
        }

        private void UpdateSelectionIndex(InteractionHandler handler, InteractionAction[] actions) {
            if (handler == null || actions == null || actions.Length <= 0)
                return;

            if (handler.IsSelectingNextInteraction) currentSelectedIndex++;
            else if (handler.IsSelectingPreviousInteraction) currentSelectedIndex--;

            currentSelectedIndex = Mathf.Clamp(currentSelectedIndex, 0, actions.Length - 1);
           
            if (previousSelectedIndex != currentSelectedIndex) {
                previousSelectedIndex = currentSelectedIndex;
                onSelectionIndexChanged?.Invoke(currentSelectedIndex);
            }
        }

        protected void SetInteractionType(InteractionType type) {
            this.Type = type;
        }

        public virtual void AddInteractionAction(InteractionAction action) {
            if (action == null || actions == null || actions.Contains(action))
                return;

            actions?.Add(action);
            onInteractionActionAdded?.Invoke(action);
        }

        public virtual void RemoveInteractionAction(InteractionAction action)
        {
            if (action == null || actions == null || !actions.Contains(action))
                return;

            actions?.Remove(action);
            onInteractionActionRemoved?.Invoke(action);
        }

        public void AddOnInteractionActionAdded(UnityAction<InteractionAction> action)
        {
            if(action != null) onInteractionActionAdded?.AddListener(action);
        }
        public void RemoveOnInteractionActionAdded(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteractionActionAdded?.RemoveListener(action);
        }

        public void AddOnInteractionActionRemoved(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteractionActionRemoved?.AddListener(action);
        }
        public void RemoveOnInteractionActionRemoved(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteractionActionRemoved?.RemoveListener(action);
        }

        public void AddOnSelectionIndexChangedListener(UnityAction<int> action)
        {
            if (action != null) onSelectionIndexChanged?.AddListener(action);
        }
        public void RemoveOnSelectionIndexChangedListener(UnityAction<int> action)
        {
            if (action != null) onSelectionIndexChanged?.RemoveListener(action);
        }

        public void AddOnSelectedListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onSelected?.AddListener(action);
        }
        public void RemoveOnSelectedListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onSelected?.RemoveListener(action);
        }

        public void AddOnUnselectedListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onUnselected?.AddListener(action);
        }
        public void RemoveOnUnelectedListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onUnselected?.RemoveListener(action);
        }

        public void Select(InteractionHandler handler) {
            onSelected?.Invoke(handler);
        }
        public void Unselect(InteractionHandler handler) {
            onUnselected?.Invoke(handler);
        }
    }
}
