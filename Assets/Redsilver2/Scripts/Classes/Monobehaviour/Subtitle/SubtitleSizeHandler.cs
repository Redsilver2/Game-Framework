using System;
using UnityEngine;

namespace RedSilver2.Framework.Subtitles
{
    public class SubtitleSizeHandler : DialogEventHandler
    {
        [SerializeField] private Vector3 screenSpaceSubtitleSize;
        [SerializeField] private Vector3 worldSpaceSubtitleSize;

        [Space]
        [SerializeField] private float sizeUpdateSpeed;

        protected sealed override void SetDefaultEvents(SubtitleManager manager, bool isAddingEvents) {

        }

        private void UpdateScreenSpaceSubtitle(SubtitleDisplayer[] handlers) {
            UpdateSubtitleHandlers(handlers, screenSpaceSubtitleSize);
        }

        private void UpdateWorldSpaceSubtitle(SubtitleDisplayer[] handlers){
            UpdateSubtitleHandlers(handlers, worldSpaceSubtitleSize);
        }

        private void UpdateSubtitleHandlers(SubtitleDisplayer[] handlers, Vector3 size) {
            if(handlers == null) return; 

            foreach(SubtitleDisplayer handler in handlers) {
                if (handler == null) continue;
                Transform transform = handler.transform;

                transform.localScale = Vector3.Lerp(transform.localScale, size, Time.deltaTime * sizeUpdateSpeed);
            }
        }
    }
}
