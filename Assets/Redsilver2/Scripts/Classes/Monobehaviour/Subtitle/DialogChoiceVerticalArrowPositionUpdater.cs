using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public class DialogChoiceVerticalArrowPositionUpdater : DialogChoiceVerticalPositionUpdater {

        [Space]
        [SerializeField] private float horizontalSpacingIncrement;

        protected override void UpdateNextHandlerPosition(DialogChoiceHandler handler, int index) {
            base.UpdateNextHandlerPosition(handler, index);
            int sign = IsSelectedIndex(0) ? -1 : 1;


            if(IsLeftArrayElement(index) || IsSelectedIndex(index)) {
                SetCurrentPosition(handler, GetCurrentPosition(handler) + Vector3.right * sign * horizontalSpacingIncrement);
            }
            else {
                SetCurrentPosition(handler, GetCurrentPosition(handler) + Vector3.right * sign * -horizontalSpacingIncrement);
            }
        }
    }
}
