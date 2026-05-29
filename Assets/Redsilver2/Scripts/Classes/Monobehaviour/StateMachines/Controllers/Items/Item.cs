using RedSilver2.Framework.Items;
using RedSilver2.Framework.Player.Inventories;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class Item : InteractionModule
    {
        [SerializeField] private ItemData data;
        
        private Inventory         inventory;
        private MeshRenderer[]    renderers;

        private UnityEvent onAdded, onRemoved, onDisabled, onEnabled;
        private ItemType type;

        public  ItemType Type => type; 
        public    ItemData Data => data;
        protected Inventory Inventory => inventory;

        protected override void Awake() 
        {
            onAdded       = new UnityEvent();
            onRemoved     = new UnityEvent();
            
            onDisabled    = new UnityEvent();
            onEnabled     = new UnityEvent();

            type          = GetItemType();
            renderers     = transform.root.GetComponentsInChildren<MeshRenderer>();

            AddOnAddedListener(() => {
                SetInteractionColliderVisibility(false);
                SetMeshRenderersVisibility(false);
            });

            AddOnRemovedListener(() => {
                SetInteractionColliderVisibility(true);
                SetMeshRenderersVisibility(true);
            });
        }

        protected virtual void Start() {
            enabled = false;
        }

        private void OnEnable()
        {
            onEnabled?.Invoke();
        }

        private void OnDisable()
        {
            onDisabled?.Invoke();
        }

        protected void SetInteractionColliderVisibility(bool isVisible) {

        }

        public void SetMeshRenderersVisibility(bool isVisible) {
            foreach (MeshRenderer renderer in renderers.Where(x => x != null))
                renderer.enabled = isVisible;
        }

        public MeshRenderer GetMeshRenderer(string name)
        {
            var results = renderers.Where(x => x != null).Where(x => x.name.ToLower().Equals(name.ToLower()));
            return results.Count() > 0 ? results.First() : null;
        }


        public void AddOnAddedListener(UnityAction action) {
            if (action != null) onAdded?.AddListener(action);
        }

        public void RemoveOnAddedListener(UnityAction action)
        {
            if (action != null) onAdded?.RemoveListener(action);
        }

        public void AddOnRemovedListener(UnityAction action) {
            if(action != null) onRemoved?.AddListener(action);
        }

        public void RemoveOnRemovedListener(UnityAction action)
        {
            if (action != null) onRemoved?.RemoveListener(action);
        }

        public void AddOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.AddListener(action);
        }

        public void RemoveOnDisabledListener(UnityAction action)
        {
            if (action != null) onDisabled?.RemoveListener(action);
        }

        public void AddOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.AddListener(action);
        }

        public void RemoveOnEnabledListener(UnityAction action)
        {
            if (action != null) onEnabled?.RemoveListener(action);
        }

        protected abstract ItemType GetItemType();
    }
}
