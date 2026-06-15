using RedSilver2.Framework.Interactions.Actions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactableName;
        [SerializeField] private Transform uiParent;

        private int currentSelectedIndex = 0, previousSelectedIndex = -1;

        private Collider _collider;
        private IEnumerator selectionUpdateCoroutine;

        private List<InteractionAction> actions;

        private UnityEvent<int> onSelectionIndexChanged;
        private UnityEvent<InteractionAction> onInteracionActionAdded, onInteracionActionRemoved;
        private UnityEvent<InteractionHandler> onSelected, onUnselected;

        public string InteractableName => interactableName;
        public InteractionType Type { get; private set; }

        public Transform UIParent => uiParent;
        public Collider Collider => _collider;

        protected virtual void Awake()
        {
            _collider    = GetComponent<Collider>();
            actions      = new List<InteractionAction>();

            onSelected   = new UnityEvent<InteractionHandler>();
            onUnselected = new UnityEvent<InteractionHandler>();

            onInteracionActionAdded = new UnityEvent<InteractionAction>();
            onInteracionActionRemoved = new UnityEvent<InteractionAction>();

            AddOnSelectedListener(StartSelectionUpdate);
            AddOnUnselectedListener(StopSelectionUpdate);

            gameObject.layer = GameManager.InteractionLayer;
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        private void OnEnable() {
            InteractionHandler.AddInteractionModuleInstance(_collider, this);
        }

        private void OnDisable() {
            InteractionHandler.RemoveInteractionModuleInstance(_collider);
        }

        private void StartSelectionUpdate(InteractionHandler handler) {
            StopSelectionUpdate(handler);
            selectionUpdateCoroutine = SelectionUpdate(handler);
            StartCoroutine(selectionUpdateCoroutine);
        }

        private void StopSelectionUpdate(InteractionHandler handler) {
            if (selectionUpdateCoroutine != null)
                StopCoroutine(selectionUpdateCoroutine);

            selectionUpdateCoroutine = null;
        }

        private IEnumerator SelectionUpdate(InteractionHandler handler) {
            onSelectionIndexChanged?.Invoke(currentSelectedIndex);

            while (handler != null) {
                Interaction[] interactions = GetInteractions(true);

                if (interactions ==  null || interactions.Length <= 0) {
                    yield return null;
                    continue;
                }

                UpdateSelectionIndex(handler, interactions);
                interactions[currentSelectedIndex]?.Interact(handler);

                Debug.Log(interactions[currentSelectedIndex].Name);
                yield return null;
            }
        }

        private void UpdateSelectionIndex(InteractionHandler handler, Interaction[] interactions) {
            if (handler == null || interactions == null || interactions.Length <= 0)
                return;

            if (handler.IsSelectingNextInteraction) currentSelectedIndex++;
            else if (handler.IsSelectingPreviousInteraction) currentSelectedIndex--;

            currentSelectedIndex = Mathf.Clamp(currentSelectedIndex, 0, interactions.Length - 1);
           
            if (previousSelectedIndex != currentSelectedIndex) {
                previousSelectedIndex = currentSelectedIndex;
                onSelectionIndexChanged?.Invoke(currentSelectedIndex);
            }
        }

        protected void SetInteractionType(InteractionType type) {
            this.Type = type;
        }

        public virtual void AddInteractionAction(InteractionAction interaction) {
            if (interaction == null || actions == null || actions.Contains(interaction))
                return;

            actions?.Add(interaction);
        }

        public virtual void RemoveInteractionAction(InteractionAction interaction)
        {
            if (interaction == null || actions == null || !actions.Contains(interaction))
                return;

            actions?.Remove(interaction);
        }

        public void AddOnInteracionActionAdded(UnityAction<InteractionAction> action)
        {
            if(action != null) onInteracionActionAdded?.AddListener(action);
        }
        public void RemoveOnInteracionActionAdded(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteracionActionAdded?.RemoveListener(action);
        }

        public void AddOnInteracionActionRemoved(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteracionActionRemoved?.AddListener(action);
        }
        public void RemoveOnInteracionActionRemoved(UnityAction<InteractionAction> action)
        {
            if (action != null) onInteracionActionRemoved?.RemoveListener(action);
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

        public Interaction[] GetInteractions() {
            List<Interaction> interactions = new List<Interaction>();
            if (actions == null) return interactions.ToArray(); 

            foreach(InteractionAction action in actions.Where(x => x != null)) {
                if (action == null) continue;
                interactions?.Add(action.Interaction);
            }

            return interactions.ToArray();
        }

        public Interaction[] GetInteractions(bool getEnabledInteractions) {
            Interaction[] interactions = GetInteractions();
          
            if (interactions == null) return new Interaction[0];
            else if (getEnabledInteractions) return interactions.Where(x => x != null).Where(x => x.IsEnabled).ToArray();
            
            return interactions;
           
        }
    }
}
