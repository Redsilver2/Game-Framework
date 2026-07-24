
namespace RedSilver2.Framework.Interactions.Actions.Modules {
    public abstract class RotatableDoorEvent : InteractionModuleEvent
    {
        private RotatableDoor door;

        protected sealed override void Start()
        {
            SetEvents(door, true);
        }

        protected sealed override void OnDisable()
        {
            SetEvents(door, false);
        }

        protected sealed override void OnEnable()
        {
            SetEvents(door, true);
        }


        protected sealed override void SetInteractionModule(InteractionModule module)
        {
            door = module as RotatableDoor;
        }

        protected abstract void SetEvents(RotatableDoor door, bool isAddingEvent);
    }
}
