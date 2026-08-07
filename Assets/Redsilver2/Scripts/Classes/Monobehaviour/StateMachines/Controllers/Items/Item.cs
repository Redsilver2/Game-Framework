using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Inventories;
using RedSilver2.Framework.StateMachines;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public abstract class Item : InteractionModule
    {
        private bool isDropping;
        private Inventory owner;


        private MeshRenderer[] renderers;
        private UnityEvent     onAdded, onRemoved, onDisabled, onEnabled;
        private UnityEvent     onDropped;

        private ItemType type;

        public bool IsDropping => isDropping;
        public  ItemType ItemType => type;
  



#if UNITY_EDITOR
        protected virtual void OnValidate() {

        }
#endif

        protected override void Awake() 
        {
            base.Awake();

            owner = null;
            SetInteractionType(InteractionType.Item);

            onAdded       = new UnityEvent();
            onRemoved     = new UnityEvent();
            
            onDisabled    = new UnityEvent();
            onEnabled     = new UnityEvent();

            onDropped = new UnityEvent();
            isDropping = false;

            type          = GetItemType();
            renderers     = transform.GetComponentsInChildren<MeshRenderer>();

            AddOnAddedListener  (() => { SetInteractionColliderVisibility(false); });
            AddOnRemovedListener(() => { SetInteractionColliderVisibility(true); });
        }

        private void Start()
        {
            GetComponent<EquippableItemStateMachine>()?.AddOnGroundTouchedListener(v => { isDropping = false; });
        }

        protected override void OnSelectionUpdate(InteractionHandler handler)
        {
            base.OnSelectionUpdate(handler);
            if (handler != null) {
                if (handler.IsPressed()) {
                    Take(handler.Inventory);
                }
            }
        }

        protected override void OnEnable() {
            base.OnEnable();
            owner?.AddOnItemRemovedListner(OnRemoved);

            onEnabled?.Invoke();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            owner?.RemoveOnItemRemovedListner(OnRemoved);

            onDisabled?.Invoke();
        }

        public virtual void Take(Inventory inventory)
        {
            if (isDropping) return;
            else if (owner != inventory) RemoveFromInventory();
            inventory?.AddItem(this);

            if (inventory != null) {
                if (inventory.ContainsItem(this)) {
                    owner = inventory;
                    SetIsInteractable(false);

                    onAdded?.Invoke();
                    owner?.AddOnItemRemovedListner(OnRemoved);
                }              
            }
        }

        public virtual void Drop()
        {
            if (!isDropping) {
                isDropping = true;
                onDropped?.Invoke();
            }
        }

        public virtual void RemoveFromInventory() {
            if (owner != null) {
                Inventory current = owner;
                owner = null;

                RemoveFromInventory(current);

            }
        }

        protected virtual void RemoveFromInventory(Inventory inventory) {
            if(inventory != null){
                SetIsInteractable(true);
                inventory?.RemoveItem(this);

                if (!inventory.ContainsItem(this)) onRemoved?.Invoke();
                inventory?.RemoveOnItemRemovedListner(OnRemoved);
            }
        }

        protected virtual void OnRemoved(Item item)
        {
            owner?.RemoveOnItemRemovedListner(OnRemoved);
        }

        protected void SetInteractionColliderVisibility(bool isVisible) {
            Collider collider = Collider;
            if(collider != null) collider.enabled = isVisible;
        }

        public void SetMeshRenderersVisibility(bool isVisible) {
            foreach (MeshRenderer renderer in renderers.Where(x => x != null))
                renderer.enabled = isVisible;
        }

        public bool IsInInventory()
        {
            return owner == null ? false : owner.ContainsItem(this);
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

        public void AddOnDroppedListener(UnityAction action)
        {
            if (action != null) onDropped?.AddListener(action);
        }

        public void RemoveOnDroppedListener(UnityAction action)
        {
            if (action != null) onDropped?.RemoveListener(action);
        }

        protected abstract ItemType GetItemType();
    }
}
