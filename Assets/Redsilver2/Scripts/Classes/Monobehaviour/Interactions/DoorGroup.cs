using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class DoorGroup : Door {

        [Space]
        [SerializeField] private MovableDoorGroupElement[] movableDoors;
        [SerializeField] private RotatableDoorGroupElement[] rotatableDoors;


#if UNITY_EDITOR
        private void OnValidate() {
            
        }

        private void ValidateDoors(DoorGroupElement[] elements) {
            if (elements == null) return;
            foreach (DoorGroupElement element in elements)
                element?.Validate();
        }

#endif


        protected override void OnOpen() {
            base.OnOpen();

            UpdateDoors(movableDoors);
            UpdateDoors(rotatableDoors);
        }

        protected override void OnClose() {
            base.OnClose();

            UpdateDoors(movableDoors);
            UpdateDoors(rotatableDoors);
        }

        private void UpdateDoors(DoorGroupElement[] elements) {
            if (elements == null) return;
            foreach (DoorGroupElement element in elements)
                element?.StartUpdater(this);
        }

        [System.Serializable]
        private abstract class DoorGroupElement {
            [SerializeField] private Transform anchor;

            private IEnumerator updater;

            private void StopUpdater(DoorGroup group) {
                if (updater != null) group?.StopCoroutine(updater);
                updater = null;
            }



            public void StartUpdater(DoorGroup group) {
                StopUpdater(group);
                updater = Updater(anchor, group != null ? group.IsOpen : false);
                group?.StartCoroutine(updater);
            }

            protected abstract IEnumerator Updater(Transform transform, bool isOpening);

#if UNITY_EDITOR
            public abstract void Validate();

#endif
        }

        [System.Serializable]
        private sealed class MovableDoorGroupElement : DoorGroupElement
        {
            [Space]
            [SerializeField] private float closeDuration;
            [SerializeField] private float openDuration;

            [Space]
            [SerializeField] private Vector3 openPosition;
            [SerializeField] private Vector3 closePosition;

            protected sealed override IEnumerator Updater(Transform transform, bool isOpening) {
                float t = 0f, progress = 0f;
                Vector3 currentTarget = transform != null ? transform.localPosition : Vector3.zero;
                Vector3 nextTarget    = isOpening ? openPosition : closePosition;

                while(progress < 1f && transform != null) {
                    transform.localPosition = Vector3.Lerp(currentTarget, nextTarget, progress);
                    t += Time.deltaTime;
                    progress = Mathf.Clamp01(t / (isOpening ? openDuration : closeDuration));
                    yield return null;
                }

                if (transform != null) transform.localPosition = nextTarget;
            }


#if UNITY_EDITOR
            public sealed override void Validate()
            {
                closeDuration = Mathf.Clamp(closeDuration, 0f, float.MaxValue);
                openDuration = Mathf.Clamp(openDuration, 0f, float.MaxValue);
            }
#endif
        }

        [System.Serializable]
        private sealed class RotatableDoorGroupElement : DoorGroupElement {
            [Space]
            [SerializeField] private float closeDuration;
            [SerializeField] private float openDuration;

            [Space]
            [SerializeField] private Vector3 openRotation;
            [SerializeField] private Vector3 closeRotation;

            protected sealed override IEnumerator Updater(Transform transform, bool isOpening)
            {
                float t = 0f, progress = 0f;
                Quaternion currentTarget  = transform != null ? transform.localRotation : Quaternion.identity;
                Quaternion nextTarget = isOpening ? Quaternion.Euler(openRotation) : Quaternion.Euler(closeRotation);

                while (progress < 1f && transform != null) {
                    transform.localRotation = Quaternion.Slerp(currentTarget, nextTarget, progress);
                    t += Time.deltaTime;
                    progress = Mathf.Clamp01(t / (isOpening ? openDuration : closeDuration));
                    yield return null;
                }

                if (transform != null) transform.localRotation = nextTarget;
            }


#if UNITY_EDITOR
            public sealed override void Validate()
            {
                closeDuration = Mathf.Clamp(closeDuration, 0f, float.MaxValue);
                openDuration = Mathf.Clamp(openDuration, 0f, float.MaxValue);
            }
#endif
        }
    }
}
