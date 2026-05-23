using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class AudioInteractionAction : InteractionAction
    {
        protected sealed override void SetBaseEvent(InteractionModule module, Interaction interaction, bool isAddingEvents) {
            if (module is not AudioInteractionModule || interaction == null) return;
            SetBaseEvent(module as AudioInteractionModule, interaction, isAddingEvents);
        }

        protected void SetBaseEvent(AudioInteractionModule module, Interaction interaction, bool isAddingEvents) {
            if (module == null || interaction == null) return;

            if (isAddingEvents) interaction?.AddOnInteractedListener(GetBaseEvent(module));
            else interaction?.RemoveOnInteractedListener(GetBaseEvent(module));
        }

        protected abstract UnityAction<InteractionHandler> GetBaseEvent(AudioInteractionModule module);
    }
}
