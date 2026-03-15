using RedSilver2.Framework.Items;
using RedSilver2.Framework.Player.Inventories;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private ItemData data;
        private Inventory inventory;

        private InteractionModule interactionModule;
        private MeshRenderer[]    renderers;

        private UnityEvent onAdded, onRemoved, onDisabled, onEnabled;
        private IEnumerator dropCoroutine;

        private ItemType type;

        public  ItemType Type => type; 
        public    ItemData Data => data;
        protected Inventory Inventory => inventory;

        protected virtual void Awake() {
            interactionModule = transform.root.GetComponentInChildren<InteractionModule>();
            SetInteractionModuleEvent(interactionModule, true);

            onAdded       = new UnityEvent();
            onRemoved     = new UnityEvent();
            onDisabled = new UnityEvent();
            onEnabled = new UnityEvent();

            type          = GetItemType();
            renderers     = transform.root.GetComponentsInChildren<MeshRenderer>();

            AddOnAddedListener(() => {
                SetInteractionColliderVisibility(false);
                SetMeshRenderersVisibility(false);
                CancelDrop();
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

        public void Inspect() {

        }


        protected void SetInteractionColliderVisibility(bool isVisible) {
            if (interactionModule == null || interactionModule.Collider == null) return;
            interactionModule.Collider.enabled = isVisible;
        }

        public void SetMeshRenderersVisibility(bool isVisible) {
            foreach (MeshRenderer renderer in renderers.Where(x => x != null))
                renderer.enabled = isVisible;
        }

        private void CancelDrop()
        {
            if (dropCoroutine != null) StopCoroutine(dropCoroutine);
            dropCoroutine = null;
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

        private void SetInteractionModuleEvent(InteractionModule module, bool isAddingEvent)
        {
            if (module == null) return;
            if (module is SingleInteractionModule) SetInteractionModuleEvent(module as SingleInteractionModule      , isAddingEvent);
            else                                   SetInteractionModuleEvent(module as AdvancedHoldInteractionModule, isAddingEvent);
        }

        private void SetInteractionModuleEvent(SingleInteractionModule module, bool isAddingEvent)
        {
            if (module == null) return;
            if (isAddingEvent)  module?.AddOnInteractListener(GetOnInteract(module));
            else                module?.RemoveOnInteractListener(GetOnInteract(module));
        }

        private void SetInteractionModuleEvent(AdvancedHoldInteractionModule module, bool isAddingEvent)
        {
            if (module == null) return;
            if (isAddingEvent) module?.AddOnProgressChanged(GetOnAdvancedInteract(module));
            else module?.RemoveOnProgressChanged(GetOnAdvancedInteract(module));
        }

        public void Add(Inventory inventory)
        {
            bool wasAdded = false;
            inventory?.AddItem(this, out wasAdded);
            if (wasAdded) onAdded.Invoke();
        }

        private UnityAction<InteractionHandler> GetOnInteract(InteractionModule module) {
            if (module == null) return null;

            return handler => {
                if (handler == null || inventory != null) return;
                InteractionHandlerModule owner = handler.Owner;

                if (owner == null) return;
                Add(owner.Inventory);
            };
        }

        private UnityAction<float, InteractionHandler> GetOnAdvancedInteract(AdvancedHoldInteractionModule module)
        {
            if(module == null) return null;
            UnityAction<InteractionHandler> result = GetOnInteract(module);
         
            if(result == null) return null; 

            return (progress, handler) =>
            {
              //  if (progress == 1f && !isInInventory)
               //     result?.Invoke(handler);
            };
        }

        protected abstract ItemType GetItemType();
    }
}
