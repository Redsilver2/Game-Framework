using UnityEngine;
using UnityEngine.Events;

namespace RedSilver2.Framework.Interactions.Actions
{
    public sealed class OpenDoor : DoorAction
    {
        protected override void SetInteractionEvent(Door door, Interaction interaction, bool isAddingEvent)
        {
            base.SetInteractionEvent(door, interaction, isAddingEvent);

            if (door == null) return;
            else if (isAddingEvent) {
                door?.AddOnStateChangedListener(GetOnStateChangedAction(door));
                if (!door.IsOpen && !door.IsLocked) door?.AddInteractionAction(this);
            }
            else {
                door?.RemoveOnStateChangedListener(GetOnStateChangedAction(door));
                door?.RemoveInteractionAction(this);
            }
        }

        protected sealed override UnityAction<InteractionHandler> GetOnInteractedAction(Door door)
        {
            return handler => { door?.Open(); };
        }

        private UnityAction<DoorState> GetOnStateChangedAction(Door door)
        {
            return state => {
                if (state == DoorState.Closed) door?.AddInteractionAction(this);
                else if (state == DoorState.Opened || state == DoorState.Locked) door?.RemoveInteractionAction(this);
            };
        }

    }
}
