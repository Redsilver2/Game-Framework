using UnityEngine.Events;


namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class MovableDoorAction : InteractionAction
    {
        private MovableDoor door;
        protected MovableDoor Door => door;

        protected sealed override void SetInteractionEvent(Interaction interaction, bool isAddingEvent) {
            SetInteractionEvent(door, interaction, isAddingEvent);
        }

        protected sealed override void SetInteractionModule(InteractionModule module) {
            door = module as MovableDoor;
        }

        protected virtual void SetInteractionEvent(MovableDoor door, Interaction interaction, bool isAddingEvent)
        {
            if (isAddingEvent) interaction?.AddOnInteractedListener(GetOnInteractedAction(door));
            else interaction?.RemoveOnInteractedListener(GetOnInteractedAction(door));
        }

        protected abstract UnityAction<InteractionHandler> GetOnInteractedAction(MovableDoor door);
    }
}
