using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class PositionDoor : Door
    {
        [SerializeField] private Vector3 desiredTarget;
        private Vector3 currentPosition;

        protected override void Awake()
        {
            base.Awake();
        }

        protected sealed override void OnUpdateStarted(bool isOpen, Transform anchor) {
            if(anchor != null) currentPosition = anchor.localPosition;
        }

        protected sealed override void OnUpdateEnded(bool isOpen, Transform anchor) {
            if (anchor != null) anchor.localPosition = GetDesiredPosition(isOpen);
        }

        protected sealed override void OnProgressionUpdate(bool isOpen, float progress, Transform anchor) {
            if(anchor != null) anchor.localPosition = Vector3.Lerp(currentPosition, GetDesiredPosition(isOpen), progress);
        }

        private Vector3 GetDesiredPosition(bool isOpen) { return isOpen ? desiredTarget : OriginalTarget; }

    }
}
