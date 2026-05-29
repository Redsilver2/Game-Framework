using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups
{
    public class CloseDoorSetup : DoorInteractionActionSetup
    {
        [SerializeField] private bool showWhenNecessary;
        [SerializeField] private CloseDoor action;

        private Door door;

        protected sealed override void OnDisable() {
            Disable();
        }

        protected sealed override void OnEnable() {
            Enable();
        }

        protected sealed override void SetInteractionModule(Door door)
        {
            this.door = door;
            OnEnable();
        }

        private void OnOpen()
        {
            if (showWhenNecessary) action?.Enable(door);  
        }

        private void OnClose() {
            if (showWhenNecessary) action?.Disable(door);
        }

        private void Enable()
        {
            action?.Add(door);
            door?.AddOnCloseListener(OnClose);

            door?.AddOnOpenListener(OnOpen);
            action?.Enable(door);

            if (showWhenNecessary) {
                if (door == null || !door.IsOpen) return;
                action?.Disable(door);
            }
        }

        private void Disable()
        {
            action?.Remove(door);
            door?.RemoveOnCloseListener(OnClose);
            door?.RemoveOnOpenListener(OnOpen);
        }

    }
}
