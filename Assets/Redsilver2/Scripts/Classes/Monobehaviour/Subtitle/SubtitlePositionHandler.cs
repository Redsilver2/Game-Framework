using System;
using System.Linq;
using UnityEngine;

namespace RedSilver2.Framework.Dialogs
{
    public abstract class SubtitlePositionHandler : DialogEventHandler
    {
        [Space]
        [SerializeField] private Vector3 defaultPosition;

        [Space]
        [SerializeField] private float subtitleLineSpacing = 10f;
        [SerializeField] private float spacingMultiplier   = 1f;
        [SerializeField] private float subtitleLerpSpeed   = 10f;


        protected void UpdateSubtitleHandlers(SubtitleHandler[] handlers) {
            if (handlers == null) return;

            float previousHeight = 0f;
            handlers = handlers.Where(x => x.IsUpdateStarted && !x.IsFadedIn()).Reverse().ToArray();

            for (int i = 0; i < handlers.Length; i++)
                UpdateSubtitleHandler(handlers[i], i, ref previousHeight);
        }

        protected virtual void UpdateSubtitleHandler(SubtitleHandler handler, int index, ref float previousHeight){
            if(handler == null) return;
            UpdateSubtitleHandlerParent(handler, GetParent(handler));
            UpdateSubtitleHandlersPosition(handler, index, ref previousHeight);
        }

        private void UpdateSubtitleHandlerParent(SubtitleHandler handler, Transform parent) {
            if (handler == null) return;
            Transform transform = handler.transform;
            Vector3 _default = defaultPosition;

            transform.SetParent(parent);
            transform.localPosition = Vector3.right * _default.x + Vector3.up * transform.localPosition.y + Vector3.forward * _default.z;
        }

        private void UpdateSubtitleHandlersPosition(SubtitleHandler handler, int index, ref float previousHeight) {
            if (handler == null) return;

            int       lineCount = handler.LineCount;
            Transform transform = handler.transform;

            Vector3   nextPosition  = GetNextPosition(transform, previousHeight, index, lineCount);
            transform.localPosition = Vector3.Lerp(transform.localPosition, nextPosition, Time.deltaTime * subtitleLerpSpeed);
           
            previousHeight = transform.localPosition.y;
        }

        private Vector3 GetNextPosition(Transform transform, float previousHeight, int index, int lineCount)
        {
            if(transform == null) return Vector3.zero;
            float height = subtitleLineSpacing * lineCount;

            if (index == 0) height += defaultPosition.y;
            else            height += previousHeight + spacingMultiplier;

            return Vector3.right   * transform.localPosition.x + 
                   Vector3.up      * height                    +
                   Vector3.forward * transform.localPosition.z;
        }

        protected abstract Transform GetParent(SubtitleHandler handler);
    }
}
