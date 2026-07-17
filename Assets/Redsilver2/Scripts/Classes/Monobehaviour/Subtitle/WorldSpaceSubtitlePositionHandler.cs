using RedSilver2.Framework.StateMachines.Controllers;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs {
    public sealed class WorldSpaceSubtitlePositionHandler : SubtitlePositionHandler
    {
        [Space]
        [SerializeField] private float rotationAdjustValue;
        [SerializeField] private float subtitleLookSpeed;


        protected override void UpdateSubtitleHandler(SubtitleHandler handler, int index, ref float previousHeight){
            PlayerController current = PlayerController.Current;
            if (handler == null || current == null) return;

            Transform  transform = handler.transform;
            Vector3    direction = current.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.Euler(0f, angle - rotationAdjustValue, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, target,
                                                  Time.deltaTime * subtitleLookSpeed);

            base.UpdateSubtitleHandler(handler, index, ref previousHeight);
        }

        protected sealed override Transform GetParent(SubtitleHandler handler) {
            return handler == null ? null : handler.Parent;
        }

        protected sealed override void SetDefaultEvents(DialogManager manager, bool isAddingEvents) {
            if (isAddingEvents) manager?.AddOnWorldSpaceSubtitleUpdateListener(UpdateSubtitleHandlers);
            else                manager?.RemoveOnWorldSpaceSubtitleUpdateListener(UpdateSubtitleHandlers);
        }
    }
}
