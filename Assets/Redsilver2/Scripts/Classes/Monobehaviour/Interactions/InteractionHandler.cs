using RedSilver2.Framework.Inputs;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandler 
    {
        private bool isEmptySelectedInteraction;

        private readonly InteractionHandlerModule owner;
        private          InteractionModule        selectedInteraction;

        private UnityEvent<InteractionModule> onSelected;
        private UnityEvent<InteractionModule> onUnselected;

        public bool IsHeld     => owner == null ? false : owner.IsHeld();
        public bool IsPressed  => owner == null ? false : owner.IsPressed();
        public bool IsReleased => owner == null ? false : owner.IsReleased();

        public bool IsSelectingNextInteraction => InputManager.GetKeyDown(KeyboardKey.UpArrow);
        public bool IsSelectingPreviousInteraction => InputManager.GetKeyDown(KeyboardKey.DownArrow);

        public InteractionModule CurrentInteraction => selectedInteraction;
        public InteractionHandlerModule Owner => owner;

        private readonly static Dictionary<Collider, InteractionModule> interactionModuleInstances = new Dictionary<Collider, InteractionModule>();
        private readonly static UnityEvent<InteractionModule> onInteractionModuleAdded             = new UnityEvent<InteractionModule>();
        private readonly static UnityEvent<InteractionModule> onInteractionModuleRemoved           = new UnityEvent<InteractionModule>();

        protected InteractionHandler(InteractionHandlerModule module) {
            this.owner              = module;
            this.isEmptySelectedInteraction = false;

            this.onSelected = new UnityEvent<InteractionModule>();
            this.onUnselected = new UnityEvent<InteractionModule>();

            AddOnUnselectedListener(interaction => {
                interaction?.Unselect(this);
                this.selectedInteraction = null;
            });

            AddOnSelectedListener(interaction => {
                this.selectedInteraction = interaction;
                interaction?.Select(this);
                Debug.Log("Selected: " + interaction);
            });
        }
  
        public void Update()
        {
            if (owner == null) return;
            InteractionModule interactionModule = GetInteractionModuleInstance(GetCollider(owner.InteractionRange));

            if (owner.CanInteract(interactionModule)) {
    

                if (!IsSelectedInteraction(interactionModule)) {
                    isEmptySelectedInteraction = false;
                    SetSelectedInteraction(interactionModule);
                }
            }
            else if(!isEmptySelectedInteraction) {
                isEmptySelectedInteraction = true;
                SetSelectedInteraction(null);
            }
        }

        private void SetSelectedInteraction(InteractionModule module)
        {
            onUnselected.Invoke(selectedInteraction);
            if(module != null) onSelected?.Invoke(module);
        }

        private bool IsSelectedInteraction(InteractionModule module)
        {
            if(selectedInteraction == null || module == null) return false;
            return selectedInteraction.Equals(module);
        }

        public void AddOnSelectedListener(UnityAction<InteractionModule> action) {
            if (action != null) onSelected?.AddListener(action);    
        }

        public void RemoveOnSelectedListener(UnityAction<InteractionModule> action) {
            if (action != null) onSelected?.RemoveListener(action);
        }

        public void AddOnUnselectedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onUnselected?.AddListener(action);
        }

        public void RemoveOnUnselectedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onUnselected?.RemoveListener(action);
        }


        protected abstract Collider GetCollider(float interactionRange);

        public static void AddOnInteractionModuleAddedListener(UnityAction<InteractionModule> action) {
            if(action != null) onInteractionModuleAdded?.AddListener(action);
        }

        public static void RemoveOnInteractionModuleAddedListener(UnityAction<InteractionModule> action) {
            if (action != null) onInteractionModuleAdded?.RemoveListener(action);
        }


        public static void AddOnInteractionModuleRemovedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onInteractionModuleRemoved?.AddListener(action);
        }

        public static void RemoveOnInteractionModuleRemovedListener(UnityAction<InteractionModule> action)
        {
            if (action != null) onInteractionModuleRemoved?.RemoveListener(action);
        }

        public static InteractionModule GetInteractionModuleInstance(Collider collider)
        {
            if (interactionModuleInstances == null || collider == null) return null;
            if (interactionModuleInstances.ContainsKey(collider)) return interactionModuleInstances[collider];  
            return null;
        }

        public static void AddInteractionModuleInstance(Collider collider, InteractionModule module)
        {
            if(collider != null && module != null && interactionModuleInstances != null)
            {
                if (!interactionModuleInstances.ContainsKey(collider)) {
                    interactionModuleInstances.Add(collider, module);
                    onInteractionModuleAdded?.Invoke(module);
                }
            }
        }

        public static void RemoveInteractionModuleInstance(Collider collider)
        {
            if (collider != null && interactionModuleInstances != null)
            {
                if (interactionModuleInstances.ContainsKey(collider)) {
                    onInteractionModuleRemoved?.Invoke(interactionModuleInstances[collider]);
                    interactionModuleInstances.Remove(collider);
                }
            }
        }
    }
}
