using System;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    public class DialogChoiceVerticalPositionUpdater : DialogChoicePositionUpdater {
        [Space]
        [SerializeField] private float deselectedVerticalSpacing = 0f;
        [SerializeField] private float selectedVerticalSpacing = 0f;

        protected override void UpdateNextHandlerPosition(DialogChoiceHandler handler, int index) {
            base.UpdateNextHandlerPosition(handler, index);
            if (index == 0) return;

            if (IsSelectedIndex(index) || IsSelectedIndex(index - 1)) {
                SetCurrentPosition(handler, GetCurrentPosition(handler) + Vector3.up * selectedVerticalSpacing);
            }
            else {
                SetCurrentPosition(handler, GetCurrentPosition(handler) + Vector3.up * deselectedVerticalSpacing);
            }
        }
    }
}
