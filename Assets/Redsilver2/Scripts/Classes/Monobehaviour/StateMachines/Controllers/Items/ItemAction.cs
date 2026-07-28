using RedSilver2.Framework.Interactions;
using RedSilver2.Framework.Interactions.Actions;
using UnityEngine.Events;

namespace RedSilver2.Framework.Items
{
    public abstract class ItemAction : InteractionAction
    {
        private Item item;

        protected sealed override void SetInteractionEvent(Interaction interaction, bool isAddingEvent)
        {
            SetInteractionEvent(item, interaction, isAddingEvent);
        }

        protected sealed override void SetInteractionModule(InteractionModule module) {
            item = module as Item;
        }

        protected virtual void SetInteractionEvent(Item item, Interaction interaction, bool isAddingEvent)
        {
            if (isAddingEvent) interaction?.AddOnInteractedListener(GetOnInteractedEvent(item));
            else interaction?.RemoveOnInteractedListener(GetOnInteractedEvent(item));
        }

        public abstract UnityAction<InteractionHandler> GetOnInteractedEvent(Item item);
    }
}
