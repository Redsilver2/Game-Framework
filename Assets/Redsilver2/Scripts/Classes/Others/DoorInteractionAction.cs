using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class DoorInteractionAction : InteractionAction
    {
        protected sealed override void SetBaseEvent(InteractionModule module, Interaction interaction, bool isAddingEvents)
        {
            if (module is not Door || interaction == null) return;
            SetBaseEvents(module as Door, interaction, isAddingEvents);
        }

        private void SetBaseEvents(Door module, Interaction interaction, bool isAddingEvents)
        {
            if (module == null || interaction == null) return;

            if (isAddingEvents) interaction?.AddOnInteractedListener(GetBaseEvent(module));
            else interaction?.RemoveOnInteractedListener(GetBaseEvent(module));
        }

        protected abstract UnityAction<InteractionHandler> GetBaseEvent(Door door);
    }
}
