using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public sealed class CloseDoor : DoorAction {

        protected override void SetInteractionEvent(Door door, Interaction interaction, bool isAddingEvent)
        {
            base.SetInteractionEvent(door, interaction, isAddingEvent);

            if (door == null) return;
            else if (isAddingEvent) {
                door?.AddOnStateChangedListener(GetOnStateChangedAction(door));
                if(door.IsOpen && !door.IsLocked) door?.AddInteractionAction(this);
            }
            else {
                door?.RemoveOnStateChangedListener(GetOnStateChangedAction(door));
                door?.RemoveInteractionAction(this);
            }
        }

        protected sealed override UnityAction<InteractionHandler> GetOnInteractedAction(Door door) {
            return handler => { door?.Close(); };
        }

        private UnityAction<DoorState> GetOnStateChangedAction(Door door) {
            return state => {
                if (state == DoorState.Opened) door?.AddInteractionAction(this);
                else if (state == DoorState.Closed || state == DoorState.Locked) door?.RemoveInteractionAction(this);
            };
        }
    }
}
