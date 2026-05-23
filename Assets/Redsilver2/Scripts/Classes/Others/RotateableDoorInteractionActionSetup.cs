using UnityEngine;

namespace RedSilver2.Framework.Interactions.Actions.Setups
{
    public abstract class RotateableDoorInteractionActionSetup : DoorInteractionActionSetup
    {
        private RotateableDoor rotateableDoor;
        public RotateableDoor RotateableDoor => rotateableDoor; 

        protected sealed override void OnDisable() {
            Disable(rotateableDoor);
        }

        protected sealed override void OnEnable() {
            Enable(rotateableDoor);
        }

        protected sealed override void SetInteractionModule(Door door) {
            SetInteractionModule(door as RotateableDoor);
        }

        protected virtual void SetInteractionModule(RotateableDoor door) {
            this.rotateableDoor = door;
            Enable(rotateableDoor);
        }

        protected abstract void Enable(RotateableDoor rotateableDoor);
        protected abstract void Disable(RotateableDoor rotateableDoor);
    }
}
