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

        private UnityAction<DoorStateType> GetOnStateChangedAction(Door door)
        {
            return state => {
                if (state == DoorStateType.Close) door?.AddInteractionAction(this);
                else if (state == DoorStateType.Open || state == DoorStateType.Locked) door?.RemoveInteractionAction(this);
            };
        }

    }
}
