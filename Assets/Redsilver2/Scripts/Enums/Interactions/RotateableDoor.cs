using RedSilver2.Framework.Interactions.Actions;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public abstract class RotateableDoor : Door
    {
        private Quaternion currentRotation;
        private float openRotationAngle;

        protected override void Awake()
        {
            base.Awake();

            OpenRotateableDoorAction action = new OpenRotateableDoorAction(this, new PressInteraction("Open Door"));
            action?.Enable();
            AddInteractionAction(action);
        }

        public void SetOpenRotation(float openRotationAngle) {
            this.openRotationAngle = openRotationAngle;
        }

        protected sealed override void OnUpdateStarted(bool isOpen, Transform anchor) {
            if (anchor != null) currentRotation = anchor.localRotation; 
            else                currentRotation = Quaternion.identity;
        }

        protected sealed override void OnUpdateEnded(bool isOpen, Transform anchor) {
            if(anchor != null) anchor.localRotation = GetDesiredRotation(isOpen);
        }

        protected sealed override void OnProgressionUpdate(bool isOpen, float progress, Transform anchor) {
            if(anchor != null)  
                anchor.localRotation = Quaternion.Slerp(currentRotation, GetDesiredRotation(isOpen), progress);
        }

        private Quaternion GetDesiredRotation(bool isOpen) { 
            return isOpen ? GetOpenRotation(openRotationAngle) : GetCloseRotation(); 
        }

        private Quaternion GetCloseRotation() => Quaternion.Euler(OriginalTarget);
        protected abstract Quaternion GetOpenRotation(float openRotationAngle);
    }
}
