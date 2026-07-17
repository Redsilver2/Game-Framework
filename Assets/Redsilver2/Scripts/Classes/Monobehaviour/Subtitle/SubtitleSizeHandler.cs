using System;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public class SubtitleSizeHandler : DialogEventHandler
    {
        [SerializeField] private Vector3 screenSpaceSubtitleSize;
        [SerializeField] private Vector3 worldSpaceSubtitleSize;

        [Space]
        [SerializeField] private float sizeUpdateSpeed;

        protected sealed override void SetDefaultEvents(DialogManager manager, bool isAddingEvents) {
            if (isAddingEvents) {
                manager?.AddOnScreenSpaceSubtitleUpdateListener(UpdateScreenSpaceSubtitle);
                manager?.AddOnWorldSpaceSubtitleUpdateListener(UpdateWorldSpaceSubtitle);
            }
            else {
                manager?.RemoveOnScreenSpaceSubtitleUpdateListener(UpdateScreenSpaceSubtitle);
                manager?.RemoveOnWorldSpaceSubtitleUpdateListener(UpdateWorldSpaceSubtitle);
            }
        }

        private void UpdateScreenSpaceSubtitle(SubtitleHandler[] handlers) {
            UpdateSubtitleHandlers(handlers, screenSpaceSubtitleSize);
        }

        private void UpdateWorldSpaceSubtitle(SubtitleHandler[] handlers){
            UpdateSubtitleHandlers(handlers, worldSpaceSubtitleSize);
        }

        private void UpdateSubtitleHandlers(SubtitleHandler[] handlers, Vector3 size) {
            if(handlers == null) return; 

            foreach(SubtitleHandler handler in handlers) {
                if (handler == null) continue;
                Transform transform = handler.transform;

                transform.localScale = Vector3.Lerp(transform.localScale, size, Time.deltaTime * sizeUpdateSpeed);
            }
        }
    }
}
