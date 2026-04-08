using RedSilver2.Framework.Inputs;
using RedSilver2.Framework.Inputs.Settings;
using RedSilver2.Framework.Interactions.Items;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public abstract class EquippableItemAction : MonoBehaviour {

        [SerializeField] private float actionDelay;
        [SerializeField] private SingleInputSettings settings;

        private UnityEvent onEnabled, onDisabled;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            actionDelay = Mathf.Clamp(actionDelay, 0f, float.MaxValue);
        }
#endif


        protected virtual void Awake()
        {
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

        public void ResetActions()
        {

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


        public void EnableActions()
        {
            if (settings == null) return;
        }

        public void DisableActions()
        {
            if (settings == null) return;
        }


        public abstract bool CanUpdate();
        protected abstract void SetItem(EquippableItem item);
    }
}
