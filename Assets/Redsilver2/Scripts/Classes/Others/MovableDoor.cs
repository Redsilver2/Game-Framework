using System.Collections;
using UnityEngine;

namespace RedSilver2.Framework.Interactions
{
    public sealed class MovableDoor : Door
    {
        [Space]
        [SerializeField] private Transform anchorPoint;

        [Space]
        [SerializeField] private float openDuration;
        [SerializeField] private float closeDuration;

        [Space]
        [SerializeField] private Vector3 closePosition;
        [SerializeField] private Vector3 openPosition;
  
        private IEnumerator positionUpdater;

     

        public void SetOpenPosition(Vector3 position) {
            this.openPosition = position;
        }

        public void SetClosePosition(Vector3 position)
        {
            this.closePosition = position;
        }

        public void SetOpenDuration(float duration) { this.openDuration = duration; }
        public void SetCloseDuration(float duration) { this.closeDuration = duration; }

        protected sealed override void OnOpen()
        {
            base.OnOpen();
            StartPositionUpdate();
        }

        protected sealed override void OnClose()
        {
            base.OnClose();
            StartPositionUpdate();
        }

        private void StopPositionUpdate()
        {
            if (positionUpdater != null) StopCoroutine(positionUpdater);
            positionUpdater = null;
        }

        private void StartPositionUpdate() {
            StopPositionUpdate();
            positionUpdater = UpdatePosition();
            StartCoroutine(positionUpdater);
        }

        private IEnumerator UpdatePosition()
        {
            float t = 0, progress = 0f;
            Vector3 currentPosition = anchorPoint != null ? anchorPoint.localPosition : Vector3.zero;
            Vector3 targetPosition  = IsOpen ? openPosition : closePosition;

            while (progress < 1f && anchorPoint != null) {
                anchorPoint.localPosition = Vector3.Lerp(currentPosition, targetPosition, progress);
                progress = Mathf.Clamp01(t / (IsOpen ? openDuration : closeDuration));

                t += Time.deltaTime;
                yield return null;
            }

            if (anchorPoint != null) anchorPoint.localPosition = targetPosition;
        }

    }
}
