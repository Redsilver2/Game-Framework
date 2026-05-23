using UnityEngine;
using UnityEngine.Events;
namespace RedSilver2.Framework.Interactions.Actions
{
    public abstract class RotateableDoorInteractionAction : DoorInteractionAction
    {
        protected sealed override UnityAction<InteractionHandler> GetBaseEvent(Door door)
        {
            if(door == null) return null;
            return GetBaseEvent(door as RotateableDoor);
        }

        protected abstract UnityAction<InteractionHandler> GetBaseEvent(RotateableDoor door);
    }
}