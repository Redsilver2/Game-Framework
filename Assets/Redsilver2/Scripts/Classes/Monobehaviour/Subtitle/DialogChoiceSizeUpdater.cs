using System;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public sealed class DialogChoiceSizeUpdater : DialogChoiceSelection
    {
        [SerializeField] private Vector3 deselectedSize;
        [SerializeField] private Vector3 selectedSize;

        [Space]
        [SerializeField] private float sizeUpdateSpeed;

#if UNITY_EDITOR
        private void OnValidate() {
            sizeUpdateSpeed = Mathf.Clamp(sizeUpdateSpeed, 1f, float.MaxValue);
        }
#endif
        protected sealed override void UpdateDeselected(DialogChoiceHandler handler, int index) {
            UpdateSize(handler, deselectedSize);
        }

        protected sealed override void UpdateSelected(DialogChoiceHandler handler, int index) {
            UpdateSize(handler, selectedSize);
        }
        protected sealed override void UpdateLeftDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex)
        {
            UpdateDeselected(handler, currentIndex);
        }

        protected sealed override void UpdateRightDeselected(DialogChoiceHandler handler, int currentIndex, int seperatedArrayIndex)
        {
            UpdateDeselected(handler, currentIndex);
        }


        private void UpdateSize(DialogChoiceHandler handler, Vector3 size) {
            if (handler == null) return;
            Transform transform  = handler.transform;
            transform.localScale = Vector3.Lerp(transform.localScale, size, Time.deltaTime * sizeUpdateSpeed);
        }
    }
}
