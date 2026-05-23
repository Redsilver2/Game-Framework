using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public class InteractionModule : MonoBehaviour
    {
        [SerializeField] private string interactableName;
        [SerializeField] private Transform uiParent;

        private Collider _collider;
        private IEnumerator selectionUpdateCoroutine;

        private List<Interaction> interactions;

        private UnityEvent<int> onSelectionIndexChanged;
        private UnityEvent<InteractionHandler> onSelected, onUnselected;
        private UnityEvent<Interaction> onInteractionAdded, onInteractionRemoved;

        public string InteractableName => interactableName;
        public Transform UIParent => uiParent;
        public InteractionType Type { get; private set; }

        public Collider Collider => _collider;
        public Interaction[] Interactions {
            get {
                return interactions != null ? interactions.ToArray() : new Interaction[0];
            }
        }

        protected virtual void Awake()
        {
            _collider    = GetComponent<Collider>();
            interactions = new List<Interaction>();

            onSelected   = new UnityEvent<InteractionHandler>();
            onUnselected = new UnityEvent<InteractionHandler>();

            onInteractionAdded   = new UnityEvent<Interaction>();
            onInteractionRemoved = new UnityEvent<Interaction>();

            InteractionHandler.AddInteractionModuleInstance(_collider, this);

            AddOnSelectedListener(StartSelectionUpdate);
            AddOnUnselectedListener(StopSelectionUpdate);

            gameObject.layer = GameManager.InteractionLayer;
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
            int currentSelected = 0, previousSelected = 0;
            onSelectionIndexChanged?.Invoke(currentSelected);

            while (handler != null) {

                Interaction[] interactions = GetActifInteractions();

                if (interactions ==  null || interactions.Length <= 0) {
                    yield return null;
                    continue;
                }

                UpdateSelectionIndex(handler, interactions, ref currentSelected, ref previousSelected);
                interactions[currentSelected]?.Interact(handler);
                yield return null;
            }
        }

        private void UpdateSelectionIndex(InteractionHandler handler, Interaction[] interactions, ref int current, ref int previous) {
            if (handler == null || interactions == null || interactions.Length <= 0)
                return;

            if (handler.IsSelectingNextInteraction) current++;
            else if (handler.IsSelectingPreviousInteraction) current--;

            current = Mathf.Clamp(current, 0, interactions.Length - 1);
            Debug.Log(current + " " + interactions[current].Name);

            if (current != previous) onSelectionIndexChanged?.Invoke(current);
        }

        protected void SetInteractionType(InteractionType type)
        {
            this.Type = type;
        }

        public virtual void AddInteraction(Interaction interaction)
        {
            if (interaction == null || interactions == null || interactions.Contains(interaction))
                return;

            interactions?.Add(interaction);
            onInteractionAdded?.Invoke(interaction);
        }

        public virtual void RemoveInteraction(Interaction interaction)
        {
            if (interaction == null || interactions == null || !interactions.Contains(interaction))
                return;

            interactions?.Remove(interaction);
            onInteractionRemoved?.Invoke(interaction);
        }

        public void AddOnInteractionAddedListener(UnityAction<Interaction> action) {
            if (action != null) onInteractionAdded?.AddListener(action);
        }

        public void RemoveOnInteractionAddedListener(UnityAction<Interaction> action)
        {
            if (action != null) onInteractionAdded?.RemoveListener(action);
        }

        public void AddOnInteractionRemovedListener(UnityAction<Interaction> action)
        {
            if (action != null) onInteractionRemoved?.AddListener(action);
        }

        public void RemoveOnInteractionRemovedListener(UnityAction<Interaction> action)
        {
            if (action != null) onInteractionRemoved?.RemoveListener(action);
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

        public Interaction GetInteraction(int index) {
            if (interactions == null || index < 0 || index >= interactions.Count)
                return null;

            return interactions[index];
        }

         public Interaction[] GetActifInteractions() {
            if (interactions == null) return null;
            return interactions.Where(x => x != null).Where(x => x.IsEnabled).ToArray();
        }
    }
}
