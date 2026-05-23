using RedSilver2.Framework.Interactions.Settings;
using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class InteractionAction : ScriptableObject
    {
        [SerializeField] private InteractionSetting setting;

        public virtual void Add(InteractionModule module) {
            setting?.Add(module, this);
            SetBaseEvents(module, true);
        }

        public void Remove(InteractionModule module)
        {
            setting?.Remove(module, this);
            SetBaseEvents(module, false);
        }

        public void Enable(InteractionModule module)
        {
            Interaction interaction = InteractionSetting.GetInteraction(module, this);
            interaction?.Enable();
            module?.AddInteraction(interaction);
        }

        public void Disable(InteractionModule module)
        {
            Interaction interaction = InteractionSetting.GetInteraction(module, this);
            interaction?.Disable();
            module?.RemoveInteraction(interaction);
        }

        protected virtual void SetBaseEvents(InteractionModule module, bool isAddingEvents)
        {
            if (setting == null || module == null) return;
            Interaction interaction = InteractionSetting.GetInteraction(module, this);
            
            if (interaction == null) return;
            SetBaseEvent(module, interaction, isAddingEvents);
        }

        protected abstract void SetBaseEvent(InteractionModule module, Interaction interaction, bool isAddingEvents);
        public abstract string GetInteractionName();
    }
}
