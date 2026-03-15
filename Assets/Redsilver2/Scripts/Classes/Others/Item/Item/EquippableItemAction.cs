using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Interactions.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public abstract class EquippableItemAction : MonoBehaviour {

        [SerializeField] private float actionDelay;

        private List<InputAction> inputActions;
        private UnityEvent onEnabled, onDisabled;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            actionDelay = Mathf.Clamp(actionDelay, 0f, float.MaxValue);
        }
#endif


        protected virtual void Awake()
        {
            inputActions = new List<InputAction>();
            onEnabled    = new UnityEvent();
            onDisabled   = new UnityEvent();
        }


        protected virtual void Start() {
            SetItem(GetEquippableItem());
        }

    

        private void OnDisable()
        {
            onDisabled?.Invoke();
        }

        private void OnEnable()
        {
            onEnabled?.Invoke();
        }

        public void UpdateActions(ref float actionDelay, out bool isExecuted) {
            isExecuted = false;
            if (inputActions == null || inputActions.Count == 0) return;

            foreach(var action in inputActions) action?.Update();

            if(inputActions.Where(x => x.IsExecuted).Count() == inputActions.Count()) {
                actionDelay = this.actionDelay;
                isExecuted = true;
            }
        }

        public void ResetActions()
        {
            if (inputActions == null) return;
            foreach (var action in inputActions) action?.Reset();
        }

        private EquippableItem GetEquippableItem()
        {
            if(transform.root != null)
                return transform.root.GetComponentInChildren<EquippableItem>();

            return transform.GetComponent<EquippableItem>();
        }

        public void AddOnEnabledListener(UnityAction action)
        {
            if(action != null) onEnabled?.AddListener(action);
        }

        public void RemoveOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.RemoveListener(action);
        }


        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }

        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.RemoveListener(action);
        }

        protected void AddInputAction(InputAction action) {
            if (!inputActions.Contains(action))
            {
                inputActions?.Add(action);
            }
        }

        private bool ContainsSimilarAction(InputAction action)
        {
            if (action == null || string.IsNullOrEmpty(action.Name) || inputActions == null)
                return true;

            return inputActions.Where(x => x.Compare(action.Name)).Count() > 0;
        }


        public void EnableActions()
        {
           if(inputActions != null){
                foreach (var action in inputActions)
                    action?.Enable();
           }
        }

        public void DisableActions()
        {
            if (inputActions != null)
            {
                foreach (var action in inputActions)
                    action?.Disable();
            }
        }


        public abstract bool CanUpdate();
        protected abstract void SetItem(EquippableItem item);
    }
}
