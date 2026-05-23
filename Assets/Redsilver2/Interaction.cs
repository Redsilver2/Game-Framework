using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions
{
    public abstract class Interaction {
        private string name;
        private string description;

        private bool isEnabled;

        private Sprite icon;
        private UnityEvent<InteractionHandler> onInteracted;

        public string Name => name;
        public string Description => description;
        public bool IsEnabled => isEnabled; 
        public Sprite Icon => icon;

        protected Interaction(string name) {
            this.name         = name;
            this.description  = string.Empty;

            this.icon         = null;
            this.onInteracted = new UnityEvent<InteractionHandler>();

            this.isEnabled = false;
        }

        protected Interaction(string name, string description)
        {
            this.name = name;
            this.description = description;

            this.icon = null;
            this.onInteracted = new UnityEvent<InteractionHandler>();

            this.isEnabled = false;
        }

        public void Enable()
        {
            if (isEnabled) return;
            this.isEnabled = true;
        }

        public void Disable()
        {
            if(!isEnabled) return;
            this.isEnabled = false;
        }


        public void SetName(string name) { this.name = name; }
        public void SetDescription(string description) { this.description = description; }
        public void SetIcon(Sprite icon) { this.icon = icon; }

        public virtual bool Interact(InteractionHandler handler) {
            if (handler == null || isEnabled == false) return false;
            onInteracted?.Invoke(handler);
            return true;
        }
        
        public void AddOnInteractedListener(UnityAction<InteractionHandler> action)
        {
            if (action != null) onInteracted?.AddListener(action);
        }

        public void RemoveOnInteractedListener(UnityAction<InteractionHandler> action) {
            if (action != null) onInteracted?.AddListener(action);
        }
    }
}
