using RedSilver2.Framework.Interactions.Actions;
using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class RotatableDoor : Door
    {
        [Space]
        [SerializeField] private Transform anchorPoint;

        [Space]
        [SerializeField] private float openDuration;
        [SerializeField] private float closeDuration;

        [Space]
        [SerializeField] private Vector3 closeRotation;
        [SerializeField] private Vector3 openRotation;

        private IEnumerator rotationUpdater;

        protected sealed override void OnOpen()
        {
            base.OnOpen();
            StartRotationUpdate();
        }

        protected sealed override void OnClose()
        {
            base.OnClose();
            StartRotationUpdate();
        }



        private void StopRotationUpdate()
        {
            if(rotationUpdater != null) StopCoroutine(rotationUpdater);
            rotationUpdater = null;
        }

        private void StartRotationUpdate()
        {
            StopRotationUpdate();
            rotationUpdater = RotationUpdate(IsOpen);
            StartCoroutine(rotationUpdater);
        }

        public void SetOpenDuration(float duration) { this.openDuration = duration; }
        public void SetCloseDuration(float duration) { this.closeDuration = duration; }

        public void SetCloseRotation(Vector3 rotation) {
            closeRotation = rotation;
        }
        public void SetOpenRotation(Vector3 rotation) {
              this.openRotation = rotation;
        }

        private IEnumerator RotationUpdate(bool isOpen)
        {
            float t = 0f, progress = 0f;
            Quaternion currentRotation = anchorPoint != null ? anchorPoint.localRotation : Quaternion.identity;
            Quaternion targetRotation = isOpen ?  Quaternion.Euler(openRotation) : Quaternion.Euler(closeRotation);

            while (progress < 1f && anchorPoint != null) {
                anchorPoint.localRotation = Quaternion.Slerp(currentRotation, targetRotation, progress);
                progress = Mathf.Clamp01(t / (isOpen ? openDuration : closeDuration));
            
                t += Time.deltaTime;
                yield return null;
            }

            if(anchorPoint != null) anchorPoint.localRotation = targetRotation;
        }
    }
}
