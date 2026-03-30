using RedSilver2.Framework.Inputs;
using System.Collections.Generic;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class InteractionHandler 
    {
        private KeyboardKey    keyboardKey;
        private GamepadButton  gamepadKey;

        private readonly InteractionHandlerModule owner;
        private          InteractionModule        currentInteractionModule;
        public bool IsInputHeld     => InputManager.GetKey(gamepadKey)      || InputManager.GetKey(gamepadKey); 
        public bool IsInputPressed  => InputManager.GetKeyDown(keyboardKey) || InputManager.GetKeyDown(gamepadKey);
        public bool IsInputReleased => InputManager.GetKeyUp(keyboardKey)   || InputManager.GetKeyUp(gamepadKey);
        public InteractionHandlerModule Owner => owner;

        private static Dictionary<Collider, InteractionModule> interactionModuleInstances = new Dictionary<Collider, InteractionModule>();

        protected InteractionHandler()
        {
            this.keyboardKey = KeyboardKey.E;
            this.gamepadKey  = GamepadButton.ButtonEast;
        }

        protected InteractionHandler(InteractionHandlerModule module)
        {
            this.keyboardKey = KeyboardKey.E;
            this.gamepadKey  = GamepadButton.ButtonEast;
            this.owner      = module;
        }

        protected InteractionHandler(KeyboardKey keyboardKey, GamepadButton gamepadKey, InteractionHandlerModule module)
        {
            this.keyboardKey = keyboardKey;
            this.gamepadKey  = gamepadKey;
            this.owner       = module;
        }


  
        public void Update()
        {
            if (owner != null) {
                InteractionModule interactionModule = GetInteractionModuleInstance(GetCollider(owner.InteractionRange));

                ResetTimedInteractionModule(interactionModule);
                currentInteractionModule = interactionModule;

                if(owner.CanInteract(interactionModule))
                    interactionModule?.Interact(this);
            }
        }

        private void ResetTimedInteractionModule(InteractionModule interactionModule)
        {
            if (currentInteractionModule == null || !currentInteractionModule.enabled)
                return;

            if (interactionModule != currentInteractionModule && currentInteractionModule is AdvancedHoldInteractionModule)
               (currentInteractionModule as AdvancedHoldInteractionModule).Release();
        }

        protected abstract Collider GetCollider(float interactionRange);

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
                if (!interactionModuleInstances.ContainsKey(collider))
                    interactionModuleInstances.Add(collider, module);
            }
        }

        public static void RemoveInteractionModuleInstance(Collider collider)
        {
            if (collider != null && interactionModuleInstances != null)
            {
                if (interactionModuleInstances.ContainsKey(collider))
                    interactionModuleInstances.Remove(collider);
            }
        }
    }
}
