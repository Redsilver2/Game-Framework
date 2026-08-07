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

        private Collider _collider;
        private IEnumerator selectionUpdateCoroutine;

        private UnityEvent<InteractionHandler> onSelected, onSelectionUpdate, onUnselected;

        public string InteractableName => interactableName;
        public bool   IsInteractable   => isInteractable;

        public InteractionType Type { get; private set; }

        public Transform UIParent => uiParent;
        public Collider Collider => _collider;


        protected virtual void Awake() {
            _collider                 = GetComponent<Collider>();
            onSelectionUpdate                  = new UnityEvent<InteractionHandler>();

            onSelected                = new UnityEvent<InteractionHandler>();    
            onUnselected              = new UnityEvent<InteractionHandler>();

            AddOnSelectedListener(StartSelectionUpdate);     
            AddOnUnselectedListener(StopSelectionUpdate);
           
            AddOnSelectionUpdateListener(OnSelectionUpdate);
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

        protected virtual void OnSelectionUpdate(InteractionHandler handler) { }

        private IEnumerator SelectionUpdate(InteractionHandler handler) {
            while (handler != null) {
                onSelectionUpdate?.Invoke(handler);
                yield return null;
            }
        }

        protected void SetInteractionType(InteractionType type) {
            this.Type = type;
        }

        public void AddOnSelectionUpdateListener(UnityAction<InteractionHandler> action)
        {
            if(action != null) onSelectionUpdate?.AddListener(action);   
        }
        public void RemoveOnSelectionUpdateListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onSelectionUpdate?.RemoveListener(action);
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
