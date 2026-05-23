using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups {
    public class OpenRotateableDoorSetup : RotateableDoorInteractionActionSetup
    {
        [SerializeField] private bool showWhenNecessary;
        [SerializeField] private OpenRotateableDoor action;

        protected sealed override void Disable(RotateableDoor rotateableDoor) {
            action?.Remove(rotateableDoor);        
            rotateableDoor?.RemoveOnCloseListener(OnClose);
            rotateableDoor?.RemoveOnOpenListener(OnOpen);
        }

        protected sealed override void Enable(RotateableDoor rotateableDoor) {
            action?.Add(rotateableDoor);
            rotateableDoor?.AddOnCloseListener(OnClose);
           
            rotateableDoor?.AddOnOpenListener(OnOpen);
            action?.Enable(rotateableDoor);

            if (showWhenNecessary) {
                if (rotateableDoor == null || !rotateableDoor.IsOpen) return;
                action?.Disable(rotateableDoor);
            }
        }

        private void OnClose() {
            if (showWhenNecessary) action?.Enable(RotateableDoor);
        }

        private void OnOpen() {
            if (showWhenNecessary) action?.Disable(RotateableDoor);
        }
    }
}
