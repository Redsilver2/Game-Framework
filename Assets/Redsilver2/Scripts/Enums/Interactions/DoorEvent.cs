using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Modules
{
    public abstract class DoorEvent : InteractionModuleEvent
    {
        private Door door;

        protected sealed override void Start()  {
            SetEvents(door, true);
        }

        protected sealed override void OnDisable() {
            SetEvents(door, false);
        }

        protected sealed override void OnEnable() {
            SetEvents(door, true);
        }


        protected sealed override void SetInteractionModule(InteractionModule module) {
            door = module as Door;
        }

        protected abstract void SetEvents(Door door, bool isAddingEvent);
    }
}
