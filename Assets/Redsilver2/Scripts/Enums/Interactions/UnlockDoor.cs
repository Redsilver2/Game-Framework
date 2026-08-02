using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions {
    public sealed class UnlockDoor : DoorAction {

        protected sealed override void SetInteractionEvent(Door door, Interaction interaction, bool isAddingEvent)
        {
            base.SetInteractionEvent(door, interaction, isAddingEvent);

            if (door == null) return;
            else if (isAddingEvent)
            {
                door?.AddOnStateChangedListener(GetOnStateChangedAction(door));
                if (door.IsLocked) door?.AddInteractionAction(this);
            }
            else
            {
                door?.RemoveOnStateChangedListener(GetOnStateChangedAction(door));
                door?.RemoveInteractionAction(this);
            }
        }


        protected sealed override UnityAction<InteractionHandler> GetOnInteractedAction(Door door) {
            return handler => { door?.Unlock(); };
        }

        private UnityAction<DoorStateType> GetOnStateChangedAction(Door door)
        {
            return state => {
                if (state == DoorStateType.Locked) door?.AddInteractionAction(this);
                else if (state == DoorStateType.Unlocked) door?.RemoveInteractionAction(this);
            };
        }
    }
}
